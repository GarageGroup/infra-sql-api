using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace GarageGroup.Infra;

partial class SourceGeneratorExtensions
{
    internal static IReadOnlyCollection<DbEntityMetadata> GetDbEntityTypes(this GeneratorExecutionContext context)
    {
        var visitor = new ExportedTypesCollector(context.CancellationToken);
        visitor.VisitAssembly(context.Compilation.Assembly);

        return visitor.GetNonPrivateTypes().Select(GetDbEntityMetadata).NotNull().ToArray();
    }

    private static DbEntityMetadata? GetDbEntityMetadata(INamedTypeSymbol typeSymbol)
    {
        var dbEntityAttribute = typeSymbol.GetAttributes().FirstOrDefault(IsDbEntityAttribute);
        if (dbEntityAttribute is null)
        {
            return null;
        }

        if (typeSymbol.IsAbstract)
        {
            throw new InvalidOperationException($"DbEntity type {typeSymbol.Name} must not be abstract");
        }

        if (typeSymbol.IsStatic)
        {
            throw new InvalidOperationException($"DbEntity type {typeSymbol.Name} must not be static");
        }

        var joinData = typeSymbol.GetDbJoinData();
        var joinDataOrder = new Dictionary<DbJoinData, int>(joinData.Count);

        for (var i = 0; i < joinData.Count; i++)
        {
            joinDataOrder[joinData[i]] = i;
        }

        var tableData = new DbTableData(
            tableName: dbEntityAttribute.GetAttributeValue(0).ToStringOrElse(typeSymbol.Name),
            tableAlias: dbEntityAttribute.GetAttributeValue(1)?.ToString());

        var fields = new List<DbFieldMetadata>();
        DbExtensionFieldMetadata? extensionField = null;

        foreach (var propertySymbol in typeSymbol.GetPropertySymbols())
        {
            if (propertySymbol.GetAttributes().Any(IsDbFieldIgnoreAttribute) || propertySymbol.SetMethod is null)
            {
                continue;
            }

            if (propertySymbol.GetAttributes().Any(IsDbExtensionDataAttribute))
            {
                if (extensionField is not null)
                {
                    throw new InvalidOperationException($"DbEntity type {typeSymbol.Name} may contain no more than one DbExtensionDataAttribute");
                }

                extensionField = new(propertySymbol.Name);
                continue;
            }

            var field = new DbFieldMetadata(
                propertyName: propertySymbol.Name,
                fieldName: propertySymbol.Name,
                isNullable: propertySymbol.Type.IsNullableType(),
                castToMethod: propertySymbol.Type?.GetCastToMethod());
            
            fields.Add(field);
        }

        return new(
            fileName: typeSymbol.Name,
            entityType: new(
                displayedData: typeSymbol.GetDisplayedData(),
                isRecordType: typeSymbol.IsRecord,
                isValueType: typeSymbol.IsValueType),
            fields: fields,
            selectQueries: typeSymbol.GetPropertySymbols().SelectMany(InnerGetData).GroupBy(GetQueryName).Select(GetQueryData).ToArray(),
            extensionField: extensionField);

        static bool IsDbEntityAttribute(AttributeData attributeData)
            =>
            attributeData.AttributeClass?.IsType(DefaultNamespace, "DbEntityAttribute") is true;

        static bool IsDbFieldIgnoreAttribute(AttributeData attributeData)
            =>
            attributeData.AttributeClass?.IsType(DefaultNamespace, "DbFieldIgnoreAttribute") is true;

        static bool IsDbExtensionDataAttribute(AttributeData attributeData)
            =>
            attributeData.AttributeClass?.IsType(DefaultNamespace, "DbExtensionDataAttribute") is true;

        IEnumerable<DbSelectData> InnerGetData(IPropertySymbol propertySymbol)
            =>
            GetDbSelectData(propertySymbol, tableData, joinData);

        DbSelectQueryData GetQueryData(IGrouping<string, DbSelectData> queryGroup)
            =>
            new(
                queryName: queryGroup.Key,
                tableData: tableData,
                joinedTables: queryGroup.Select(GetJoinTable).NotNull().Distinct().OrderBy(GetJoinDataOrder).ToArray(),
                fieldNames: queryGroup.Select(GetFullFieldName).ToArray(),
                groupByFields: queryGroup.Where(IsGroupByField).Select(GetFieldName).ToArray());

        int GetJoinDataOrder(DbJoinData dbJoinData)
            =>
            joinDataOrder.TryGetValue(dbJoinData, out var index) ? index : int.MaxValue;

        static string GetQueryName(DbSelectData data)
            =>
            data.QueryName;

        static string GetFieldName(DbSelectData data)
            =>
            data.FieldName;

        static string GetFullFieldName(DbSelectData data)
            =>
            string.IsNullOrEmpty(data.AliasName) ? data.FieldName : $"{data.FieldName} AS {data.AliasName}";

        static DbJoinData? GetJoinTable(DbSelectData data)
            =>
            data.JoinTable;

        static bool IsGroupByField(DbSelectData data)
            =>
            data.GroupBy;
    }

    private static IReadOnlyList<DbJoinData> GetDbJoinData(this INamedTypeSymbol typeSymbol)
    {
        return typeSymbol.GetAttributes().Where(IsDbJoinAttribute).Select(GetDbJoinData).ToArray();

        DbJoinData GetDbJoinData(AttributeData attributeData)
            =>
            new(
                joinType: attributeData.GetAttributeValue(0) switch
                {
                    int value => value,
                    var unexpected => throw new InvalidOperationException($"An unexpected join type value: {unexpected}")
                },
                tableName: attributeData.GetAttributeValue(1).ToStringOrThrow(NotSpecifiedDbJoinTableName, true),
                tableAlias: attributeData.GetAttributeValue(2)?.ToString(),
                rawFilter: attributeData.GetAttributeValue(3).ToStringOrThrow(NotSpecifiedDbJoinRawFilter, true));


        InvalidOperationException NotSpecifiedDbJoinTableName()
            =>
            new($"DbJoin table name of DbEntity {typeSymbol.Name} must be specified");

        InvalidOperationException NotSpecifiedDbJoinRawFilter()
            =>
            new($"DbJoin raw filter value of DbEntity {typeSymbol.Name} must be specified");

        static bool IsDbJoinAttribute(AttributeData attributeData)
            =>
            attributeData.AttributeClass?.IsType(DefaultNamespace, "DbJoinAttribute") is true;
    }

    private static IEnumerable<DbSelectData> GetDbSelectData(
        IPropertySymbol propertySymbol, DbTableData tableData, IReadOnlyList<DbJoinData> joinData)
    {
        foreach (var dbSelectAttribute in propertySymbol.GetAttributes().Where(IsDbSelectAttribute))
        {
            var tableName = dbSelectAttribute.GetAttributeValue(1, "TableName")?.ToString();
            DbJoinData? joinTable = null;

            if (string.IsNullOrEmpty(tableName) is false && tableData.IsNameMatched(tableName) is false)
            {
                joinTable = joinData.FirstOrDefault(IsNameMatched) ?? throw NotFoundTableException();
            }

            var fieldName = dbSelectAttribute.GetAttributeValue(2, "FieldName")?.ToString();
            string? aliasName = null;

            if (string.IsNullOrEmpty(fieldName))
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    fieldName = propertySymbol.Name;
                }
                else
                {
                    fieldName = tableName + "." + propertySymbol.Name;
                }
            }
            else if (string.Equals(fieldName, propertySymbol.Name, StringComparison.InvariantCulture) is false)
            {
                aliasName = propertySymbol.Name;
            }

            yield return new(
                queryName: dbSelectAttribute.GetAttributeValue(0).ToStringOrThrow(NotSpecifiedQueryNameException, true),
                joinTable: joinTable,
                fieldName: fieldName ?? string.Empty,
                aliasName: aliasName,
                groupBy: dbSelectAttribute.GetAttributePropertyValue("GroupBy") is true);

            bool IsNameMatched(DbJoinData data)
                =>
                data.IsNameMatched(tableName);

            InvalidOperationException NotFoundTableException()
                =>
                new($"Table '{tableName}' was not found for the DbEntity {propertySymbol.ContainingType.Name}");
        }

        static bool IsDbSelectAttribute(AttributeData attributeData)
            =>
            attributeData.AttributeClass?.IsType(DefaultNamespace, "DbSelectAttribute") is true;

        InvalidOperationException NotSpecifiedQueryNameException()
            =>
            new($"DbSelect query name of property {propertySymbol.Name} must be specified");
    }

    private static DisplayedMethodData? GetCastToMethod(this ITypeSymbol typeSymbol)
        =>
        typeSymbol.IsValueType ? typeSymbol.GetValueTypeCastToMethod() : typeSymbol.GetRefTypeCastToMethod();

    private static DisplayedMethodData? GetRefTypeCastToMethod(this ITypeSymbol typeSymbol)
        =>
        (typeSymbol.IsSystemType("String"), typeSymbol.IsNullableType()) switch
        {
            (true, true)    => null,
            (true, false)   => new(Array.Empty<string>(), "CastToString()"),
            (false, false)  => typeSymbol.CreateCastToMethodData("CastTo"),
            _               => typeSymbol.CreateCastToMethodData("CastToNullable")
        };

    private static DisplayedMethodData? GetValueTypeCastToMethod(this ITypeSymbol typeSymbol)
    {
        if (typeSymbol.IsAnySystemType(DefaultDbFieldValueTypes))
        {
            return null;
        }

        var nullableStructType = typeSymbol.GetNullableStructType();
        if (nullableStructType is null)
        {
            return typeSymbol.CreateCastToMethodData("CastTo");
        }

        if (nullableStructType.IsAnySystemType(DefaultDbFieldValueTypes))
        {
            return null;
        }

        return nullableStructType.CreateCastToMethodData("CastToNullableStruct");
    }

    private static DisplayedMethodData CreateCastToMethodData(this ITypeSymbol typeSymbol, string methodName)
    {
        var typeData = typeSymbol.GetDisplayedData();
        return new(typeData.AllNamespaces, $"{methodName}<{typeData.DisplayedTypeName}>()");
    }
}
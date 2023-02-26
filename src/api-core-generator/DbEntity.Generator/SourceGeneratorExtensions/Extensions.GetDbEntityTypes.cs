using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace GGroupp.Infra;

partial class SourceGeneratorExtensions
{
    internal static IReadOnlyCollection<DbEntityMetadata> GetDbEntityTypes(this GeneratorExecutionContext context)
    {
        var visitor = new ExportedTypesCollector(context.CancellationToken);
        visitor.VisitNamespace(context.Compilation.GlobalNamespace);

        return visitor.GetNonPrivateTypes().Select(GetDbEntityMetadata).NotNull().ToArray();
    }

    private static DbEntityMetadata? GetDbEntityMetadata(INamedTypeSymbol typeSymbol)
    {
        if (typeSymbol.GetAttributes().Any(IsDbEntityAttribute) is false)
        {
            return null;
        }

        if (typeSymbol.IsAbstract)
        {
            throw new InvalidOperationException($"DbEntity type {typeSymbol.Name} must not be abstract");
        }

        return new(
            fileName: typeSymbol.Name,
            entityType: typeSymbol.GetDisplayedData(),
            isRecordType: typeSymbol.IsRecord,
            isValueType: typeSymbol.IsValueType,
            fields: typeSymbol.GetMembers().OfType<IPropertySymbol>().Select(GetDbFieldMetadata).NotNull().ToArray());

        static bool IsDbEntityAttribute(AttributeData attributeData)
            =>
            attributeData.AttributeClass?.IsType(DefaultNamespace, "DbEntityAttribute") is true;
    }

    private static DbFieldMetadata? GetDbFieldMetadata(IPropertySymbol propertySymbol)
    {
        var dbFieldAttribute = propertySymbol.GetAttributes().FirstOrDefault(IsDbFieldAttribute);
        if (dbFieldAttribute is null)
        {
            return null;
        }

        if (propertySymbol.SetMethod is null)
        {
            throw new InvalidOperationException($"DbField property {propertySymbol.Name} must not be readonly");
        }

        return new(
            propertyName: propertySymbol.Name,
            fieldName: dbFieldAttribute.GetAttributeValue(0).ToStringOr(propertySymbol.Name),
            isNullable: propertySymbol.Type.IsNullableType(),
            castToMethod: propertySymbol.Type?.GetCastToMethod());

        static bool IsDbFieldAttribute(AttributeData attributeData)
            =>
            attributeData.AttributeClass?.IsType(DefaultNamespace, "DbFieldAttribute") is true;
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
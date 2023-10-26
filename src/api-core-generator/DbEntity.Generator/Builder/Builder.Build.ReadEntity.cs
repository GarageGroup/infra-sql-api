using System.Linq;
using System.Text;

namespace GarageGroup.Infra;

partial class DbEntityBuilder
{
    internal static string BuildReadEntitySourceCode(this DbEntityMetadata metadata)
        =>
        new SourceBuilder(
            metadata.EntityType.DisplayedData.AllNamespaces.FirstOrDefault())
        .AddUsings(
            metadata.EntityType.DisplayedData.AllNamespaces)
        .AddUsing(
            "System",
            "GarageGroup.Infra")
        .AppendCodeLine(
            metadata.BuildHeaderLine())
        .BeginCodeBlock()
        .AppendCodeLine(
            $"public static {metadata.EntityType.DisplayedData.DisplayedTypeName} ReadEntity(IDbItem dbItem)")
        .BeginCodeBlock()
        .AppendCodeLine(
            "ArgumentNullException.ThrowIfNull(dbItem);")
        .AppendDbExtensionFieldInitializationIfNecessary(
            metadata)
        .AppendCodeLine("return new()")
        .BeginCodeBlock()
        .AppendDbFields(
            metadata)
        .EndCodeBlock(';')
        .EndCodeBlock()
        .EndCodeBlock()
        .Build();

    private static SourceBuilder AppendDbExtensionFieldInitializationIfNecessary(this SourceBuilder sourceBuilder, DbEntityMetadata metadata)
    {
        if (metadata.ExtensionField is null)
        {
            return sourceBuilder;
        }

        sourceBuilder = sourceBuilder
            .AddUsing("System.Collections.Generic")
            .AppendEmptyLine()
            .AppendCodeLine($"var {DbExtensionFieldVariableName} = new Dictionary<string, object?>();")
            .AppendCodeLine("foreach (var field in dbItem.Fields)")
            .BeginCodeBlock();

        foreach (var field in metadata.Fields)
        {
            sourceBuilder = sourceBuilder
                .AppendCodeLine($"if (string.Equals({field.FieldName.AsStringSourceCodeOrStringEmpty()}, field, StringComparison.Ordinal))")
                .BeginCodeBlock()
                .AppendCodeLine("continue;")
                .EndCodeBlock()
                .AppendEmptyLine();
        }

        return sourceBuilder
            .AppendCodeLine($"{DbExtensionFieldVariableName}[field] = dbItem.GetFieldValueOrDefault(field)?.CastToNullableObject();")
            .EndCodeBlock()
            .AppendEmptyLine();
    }

    private static SourceBuilder AppendDbFields(this SourceBuilder sourceBuilder, DbEntityMetadata metadata)
    {
        foreach (var field in metadata.Fields)
        {
            sourceBuilder = sourceBuilder.AppendDbField(field);
        }

        if (metadata.ExtensionField is not null)
        {
            sourceBuilder = sourceBuilder.AppendDbExtensionField(metadata.ExtensionField);
        }

        return sourceBuilder;
    }

    private static SourceBuilder AppendDbField(this SourceBuilder sourceBuilder, DbFieldMetadata dbField)
    {
        var builder = new StringBuilder(dbField.PropertyName).Append(" = dbItem.GetFieldValueOr");
        if (dbField.IsNullable)
        {
            builder = builder.Append("Default");
        }
        else
        {
            builder = builder.Append("Throw");
        }

        builder = builder.Append('(').Append(dbField.FieldName.AsStringSourceCodeOrStringEmpty()).Append(')');
        if (dbField.CastToMethod is null)
        {
            return sourceBuilder.AppendCodeLine(builder.Append(',').ToString());
        }

        _ = sourceBuilder.AddUsings(dbField.CastToMethod.AllNamespaces);
        if (dbField.IsNullable)
        {
            builder = builder.Append('?');
        }

        return sourceBuilder.AppendCodeLine(
            builder.Append('.').Append(dbField.CastToMethod.SourceCode).Append(',').ToString());
    }

    private static SourceBuilder AppendDbExtensionField(this SourceBuilder sourceBuilder, DbExtensionFieldMetadata dbExtensionField)
        =>
        sourceBuilder.AppendCodeLine(
            $"{dbExtensionField.PropertyName} = {DbExtensionFieldVariableName}");
}
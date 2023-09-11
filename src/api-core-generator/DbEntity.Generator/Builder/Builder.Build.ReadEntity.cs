using System.Collections.Generic;
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
        .AppendCodeLine("return new()")
        .BeginCodeBlock()
        .AppendDbFields(
            metadata.Fields)
        .EndCodeBlock(';')
        .EndCodeBlock()
        .EndCodeBlock()
        .Build();

    private static SourceBuilder AppendDbFields(this SourceBuilder sourceBuilder, IReadOnlyList<DbFieldMetadata> dbFields)
    {
        for (var i = 0; i < dbFields.Count; i++)
        {
            var dbField = dbFields[i];
            _ = sourceBuilder.AppendDbField(dbField, i + 1 == dbFields.Count);
        }

        return sourceBuilder;
    }

    private static SourceBuilder AppendDbField(this SourceBuilder sourceBuilder, DbFieldMetadata dbField, bool isLast)
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
            return sourceBuilder.AppendCodeLine(builder.AppendComma(isLast).ToString());
        }

        _ = sourceBuilder.AddUsings(dbField.CastToMethod.AllNamespaces);
        if (dbField.IsNullable)
        {
            builder = builder.Append('?');
        }

        return sourceBuilder.AppendCodeLine(
            builder.Append('.').Append(dbField.CastToMethod.SourceCode).AppendComma(isLast).ToString());
    }

    private static StringBuilder AppendComma(this StringBuilder builder, bool isLast)
    {
        if (isLast)
        {
            return builder;
        }

        return builder.Append(',');
    }
}
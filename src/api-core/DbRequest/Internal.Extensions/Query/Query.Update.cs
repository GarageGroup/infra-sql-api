using System;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildSqlQuery(this DbUpdateQuery query)
    {
        if (query.FieldValues.IsEmpty)
        {
            return string.Empty;
        }

        var setValuesBuilder = new StringBuilder();

        foreach (var fieldValue in query.FieldValues)
        {
            setValuesBuilder.AppendSeparator().Append(fieldValue.FieldName).Append(" = @").Append(fieldValue.ParameterName);
        }

        return new StringBuilder()
            .Append("UPDATE ")
            .Append(query.TableName)
            .Append(" SET ")
            .Append(setValuesBuilder)
            .Append(" WHERE ")
            .Append(query.Filter.GetFilterSqlQuery())
            .Append(';')
            .ToString();
    }

    internal static FlatArray<DbParameter> BuildParameters(this DbUpdateQuery query)
        =>
        query.FieldValues.Map(MapFieldValue).Concat(
            query.Filter.GetFilterParameters());
}
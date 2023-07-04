using System;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildSqlQuery(this DbInsertQuery query)
    {
        if (query.FieldValues.IsEmpty)
        {
            return string.Empty;
        }

        var fieldNamesBuilder = new StringBuilder();
        var insertValuesBuilder = new StringBuilder();

        foreach (var fieldValue in query.FieldValues)
        {
            fieldNamesBuilder.AppendSeparator().Append(fieldValue.FieldName);
            insertValuesBuilder.AppendSeparator().Append('@').Append(fieldValue.ParameterName);
        }

        return new StringBuilder()
            .Append("INSERT INTO ")
            .Append(query.TableName)
            .Append(" (")
            .Append(fieldNamesBuilder)
            .Append(") VALUES (")
            .Append(insertValuesBuilder)
            .Append(");")
            .ToString();
    }

    internal static FlatArray<DbParameter> BuildParameters(this DbInsertQuery query)
        =>
        query.FieldValues.Map(MapFieldValue);
}
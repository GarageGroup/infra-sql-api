using System;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildSqlQuery(this DbIfQuery query)
    {
        var queryBuilder = new StringBuilder("IF ")
            .Append(query.Condition.GetFilterSqlQuery())
            .AppendLine()
            .Append("BEGIN")
            .AppendLine()
            .Append(query.ThenQuery.GetSqlQuery())
            .AppendLine()
            .Append("END");

        if (query.ElseQuery is null)
        {
            return queryBuilder.ToString();
        }

        return queryBuilder
            .AppendLine()
            .Append("ELSE")
            .AppendLine()
            .Append("BEGIN")
            .AppendLine()
            .Append(query.ElseQuery.GetSqlQuery())
            .AppendLine()
            .Append("END")
            .ToString();
    }

    internal static FlatArray<DbParameter> BuildParameters(this DbIfQuery query)
    {
        var parameters = query.Condition.GetFilterParameters().Concat(
            query.ThenQuery.GetParameters());

        if (query.ElseQuery is null)
        {
            return parameters;
        }

        return parameters.Concat(
            query.ElseQuery.GetParameters());
    }
}
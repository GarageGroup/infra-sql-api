using System;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildSqlQuery(this DbDeleteQuery query)
        =>
        new StringBuilder(
            "DELETE FROM ")
        .Append(
            query.TableName)
        .Append(
            " WHERE ")
        .Append(
            query.Filter.GetFilterSqlQuery())
        .Append(
            ';')
        .ToString();

    internal static FlatArray<DbParameter> BuildParameters(this DbDeleteQuery query)
        =>
        query.Filter.GetFilterParameters();
}
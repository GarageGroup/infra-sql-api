using System;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildSqlQuery(this DbNotExistsFilter filter)
        =>
        new StringBuilder(
            "NOT EXISTS (")
        .Append(
            filter.SelectQuery.GetSqlQuery())
        .Append(
            ')')
        .ToString();

    internal static FlatArray<DbParameter> BuildParameters(this DbNotExistsFilter filter)
        =>
        filter.SelectQuery.GetParameters();
}
using System;
using System.Text;

namespace GarageGroup.Infra;

partial class DbQueryExtensions
{
    internal static string BuildSqlQuery(this DbExistsFilter filter)
        =>
        new StringBuilder(
            "EXISTS (")
        .Append(
            filter.SelectQuery.GetSqlQuery())
        .Append(
            ')')
        .ToString();

    internal static FlatArray<DbParameter> BuildParameters(this DbExistsFilter filter)
        =>
        filter.SelectQuery.GetParameters();
}
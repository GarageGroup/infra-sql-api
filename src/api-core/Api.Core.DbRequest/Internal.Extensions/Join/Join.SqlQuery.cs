using System;
using System.Text;

namespace GGroupp.Infra;

partial class DbRequestExtensions
{
    internal static string BuildSqlQuery(this FlatArray<DbJoinedTable> joinedTables)
    {
        if (joinedTables.IsEmpty)
        {
            return string.Empty;
        }

        var builder = new StringBuilder();

        foreach (var joinedTable in joinedTables)
        {
            if (builder.Length > 0)
            {
                builder = builder.Append(' ');
            }

            builder = builder.AppendJoinedTable(joinedTable);
        }

        return builder.ToString();
    }

    private static StringBuilder AppendJoinedTable(this StringBuilder builder, DbJoinedTable joinedTable)
        =>
        builder.Append(
            joinedTable.Type.GetName())
        .Append(
            " JOIN ")
        .Append(
            joinedTable.TableName)
        .Append(
            ' ')
        .Append(
            joinedTable.ShortName)
        .Append(
            " ON ")
        .Append(
            joinedTable.Filters.BuildSqlQuery());
}
using System;

namespace GarageGroup.Infra;

internal sealed record class DbJoinData
{
    public DbJoinData(int joinType, string tableName, string? tableAlias, string rawFilter)
    {
        JoinType = joinType;
        TableName = tableName ?? string.Empty;
        TableAlias = tableAlias;
        RawFilter = rawFilter ?? string.Empty;
    }

    public int JoinType { get; }

    public string TableName { get; }

    public string? TableAlias { get; }

    public string RawFilter { get; }

    public bool IsNameMatched(string? tableName)
        =>
        string.Equals(TableName, tableName, StringComparison.InvariantCulture) ||
        string.Equals(TableAlias, tableName, StringComparison.InvariantCulture);
}
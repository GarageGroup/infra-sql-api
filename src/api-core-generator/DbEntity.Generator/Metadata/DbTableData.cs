using System;

namespace GarageGroup.Infra;

internal sealed record class DbTableData
{
    public DbTableData(string tableName, string? tableAlias)
    {
        TableName = tableName ?? string.Empty;
        TableAlias = tableAlias;
    }

    public string TableName { get; }

    public string? TableAlias { get; }

    public bool IsNameMatched(string? tableName)
        =>
        string.Equals(TableName, tableName, StringComparison.InvariantCulture) ||
        string.Equals(TableAlias, tableName, StringComparison.InvariantCulture);
}
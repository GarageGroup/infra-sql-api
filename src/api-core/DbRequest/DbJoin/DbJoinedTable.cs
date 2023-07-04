using System;

namespace GarageGroup.Infra;

public sealed record class DbJoinedTable
{
    public DbJoinedTable(DbJoinType type, string tableName, string tableAlias, IDbFilter filter)
    {
        Type = type;
        TableName = tableName.OrEmpty();
        TableAlias = tableAlias.OrEmpty();
        Filter = filter;
    }

    public DbJoinType Type { get; }

    public string TableName { get; }

    [Obsolete("Use TableAlias instead")]
    public string? ShortName => TableAlias;

    public string? TableAlias { get; }

    public IDbFilter Filter { get; }
}
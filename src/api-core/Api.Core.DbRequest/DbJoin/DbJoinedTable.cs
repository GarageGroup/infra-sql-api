using System;

namespace GGroupp.Infra;

public sealed record class DbJoinedTable
{
    public DbJoinedTable(DbJoinType type, string tableName, string shortName, IDbFilter filter, params IDbFilter[] otherFilters)
    {
        Type = type;
        TableName = tableName ?? string.Empty;
        ShortName = shortName ?? string.Empty;
        Filters = filter.Concat(otherFilters);
    }

    public DbJoinType Type { get; }

    public string TableName { get; }

    public string ShortName { get; }

    public FlatArray<IDbFilter> Filters { get; }
}
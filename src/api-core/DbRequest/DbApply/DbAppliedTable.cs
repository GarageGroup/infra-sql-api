using System;

namespace GarageGroup.Infra;

public sealed record class DbAppliedTable
{
    public DbAppliedTable(DbApplyType type, DbSelectQuery selectQuery, string alias)
    {
        Type = type;
        SelectQuery = selectQuery;
        Alias = alias.OrEmpty();
    }

    public DbApplyType Type { get; }

    public DbSelectQuery SelectQuery { get; }

    public string Alias { get; }
}
using System;

namespace GarageGroup.Infra;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public sealed class DbJoinAttribute : Attribute
{
    public DbJoinAttribute(DbJoinType type, string tableName, string? tableAlias, string rawFilter)
    {
        Type = type;
        TableName = tableName.OrEmpty();
        TableAlias = tableAlias.OrNullIfEmpty();
        RawFilter = rawFilter.OrEmpty();
    }

    public DbJoinType Type { get; }

    public string TableName { get; }

    public string? TableAlias { get; }

    public string RawFilter { get; }
}
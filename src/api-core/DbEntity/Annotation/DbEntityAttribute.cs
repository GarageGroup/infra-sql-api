using System;

namespace GarageGroup.Infra;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public sealed class DbEntityAttribute : Attribute
{
    public DbEntityAttribute(string? tableName = null, string? tableAlias = null)
    {
        TableName = tableName;
        TableAlias = tableAlias;
    }

    public string? TableName { get; }

    public string? TableAlias { get; }
}
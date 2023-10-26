using System;

namespace GarageGroup.Infra;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class DbSelectAttribute : Attribute
{
    public DbSelectAttribute(string queryName, string? tableName = null, string? fieldName = null)
    {
        QueryName = queryName.OrEmpty();
        TableName = tableName.OrNullIfEmpty();
        FieldName = fieldName.OrNullIfEmpty();
    }

    public string QueryName { get; }

    public string? TableName { get; set; }

    public string? FieldName { get; set; }

    public bool GroupBy { get; set; }
}
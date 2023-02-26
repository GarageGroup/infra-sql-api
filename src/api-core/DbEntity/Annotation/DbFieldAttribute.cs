using System;

namespace GGroupp.Infra;

[AttributeUsage(AttributeTargets.Property)]
public sealed class DbFieldAttribute : Attribute
{
    public DbFieldAttribute(string? fieldName = null)
        =>
        FieldName = fieldName;

    public string? FieldName { get; }
}
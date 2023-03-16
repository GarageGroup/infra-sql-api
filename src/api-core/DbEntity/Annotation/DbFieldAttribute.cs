using System;

namespace GGroupp.Infra;

[Obsolete("Not to use this attribute. Use DbFieldIgnoreAttribute to ignore property if it's necessary")]
[AttributeUsage(AttributeTargets.Property)]
public sealed class DbFieldAttribute : Attribute
{
    public DbFieldAttribute(string? fieldName = null)
        =>
        FieldName = fieldName;

    public string? FieldName { get; }
}
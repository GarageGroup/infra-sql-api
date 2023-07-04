namespace GarageGroup.Infra;

internal sealed record class DbFieldMetadata
{
    public DbFieldMetadata(string propertyName, string fieldName, bool isNullable, DisplayedMethodData? castToMethod)
    {
        PropertyName = propertyName ?? string.Empty;
        FieldName = fieldName ?? string.Empty;
        IsNullable = isNullable;
        CastToMethod = castToMethod;
    }

    public string PropertyName { get; }

    public string FieldName { get; }

    public bool IsNullable { get; }

    public DisplayedMethodData? CastToMethod { get; }
}
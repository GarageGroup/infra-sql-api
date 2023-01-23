namespace GGroupp.Infra;

public sealed record class DbFieldValue
{
    public DbFieldValue(string fieldName, object? fieldValue)
    {
        FieldName = fieldName ?? string.Empty;
        FieldValue = fieldValue;
        ParameterName = FieldName;
    }

    public DbFieldValue(string fieldName, object? fieldValue, string parameterName)
    {
        FieldName = fieldName ?? string.Empty;
        FieldValue = fieldValue;
        ParameterName = parameterName ?? string.Empty;
    }

    public string FieldName { get; }

    public object? FieldValue { get; }

    public string ParameterName { get; }
}
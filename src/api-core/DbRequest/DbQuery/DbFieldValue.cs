using System;

namespace GarageGroup.Infra;

public sealed record class DbFieldValue
{
    public DbFieldValue(string fieldName, object? fieldValue)
    {
        FieldName = fieldName.OrEmpty();
        FieldValue = fieldValue;
        ParameterName = FieldName;
    }

    public DbFieldValue(string fieldName, object? fieldValue, string parameterName)
    {
        FieldName = fieldName.OrEmpty();
        FieldValue = fieldValue;
        ParameterName = parameterName.OrEmpty();
    }

    public string FieldName { get; }

    public object? FieldValue { get; }

    public string ParameterName { get; }

    public int? TimeoutInSeconds { get; init; }
}
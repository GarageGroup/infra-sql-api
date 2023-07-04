using System;

namespace GarageGroup.Infra;

public sealed record class DbLikeFilter : IDbFilter
{
    public DbLikeFilter(string fieldName, string? fieldValue, string parameterName)
    {
        FieldName = fieldName.OrEmpty();
        FieldValue = fieldValue;
        ParameterName = string.IsNullOrEmpty(parameterName) ? FieldName : parameterName;
    }

    public string FieldName { get; }

    public string? FieldValue { get; }

    public string ParameterName { get; }

    public string GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    public FlatArray<DbParameter> GetFilterParameters()
        =>
        this.BuildFilterParameters();
}
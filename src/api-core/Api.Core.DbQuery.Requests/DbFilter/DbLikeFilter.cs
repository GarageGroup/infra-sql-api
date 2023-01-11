using System;

namespace GGroupp.Infra;

public sealed record class DbLikeFilter : IDbFilter
{
    public DbLikeFilter(string fieldName, string? fieldValue, string parameterName)
    {
        FieldName = fieldName ?? string.Empty;
        FieldValue = fieldValue;
        ParameterName = string.IsNullOrEmpty(parameterName) ? FieldName : parameterName;
    }

    public string FieldName { get; }

    public string? FieldValue { get; }

    public string ParameterName { get; }

    string IDbFilter.GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    FlatArray<DbParameter> IDbFilter.GetFilterParameters()
        =>
        this.BuildFilterParameters();
}
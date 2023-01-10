using System;

namespace GGroupp.Infra;

public sealed record class DbParameterFilter : IDbFilter
{
    public DbParameterFilter(string fieldName, DbFilterOperator @operator, object? fieldValue)
    {
        FieldName = fieldName ?? string.Empty;
        Operator = @operator;
        FieldValue = fieldValue;
        ParameterName = FieldName;
    }

    public DbParameterFilter(string fieldName, DbFilterOperator @operator, object? fieldValue, string parameterName)
    {
        FieldName = fieldName ?? string.Empty;
        Operator = @operator;
        FieldValue = fieldValue;
        ParameterName = string.IsNullOrEmpty(parameterName) ? FieldName : parameterName;
    }

    public string FieldName { get; }

    public DbFilterOperator Operator { get; }

    public object? FieldValue { get; }

    public string ParameterName { get; }

    string IDbFilter.GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    FlatArray<DbParameter> IDbFilter.GetFilterParameters()
        =>
        this.BuildFilterParameters();
}
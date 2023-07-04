using System;

namespace GarageGroup.Infra;

public sealed record class DbParameterFilter : IDbFilter
{
    public DbParameterFilter(string fieldName, DbFilterOperator @operator, object? fieldValue)
    {
        FieldName = fieldName.OrEmpty();
        Operator = @operator;
        FieldValue = fieldValue;
        ParameterName = FieldName;
    }

    public DbParameterFilter(string fieldName, DbFilterOperator @operator, object? fieldValue, string parameterName)
    {
        FieldName = fieldName.OrEmpty();
        Operator = @operator;
        FieldValue = fieldValue;
        ParameterName = string.IsNullOrEmpty(parameterName) ? FieldName : parameterName;
    }

    public string FieldName { get; }

    public DbFilterOperator Operator { get; }

    public object? FieldValue { get; }

    public string ParameterName { get; }

    public string GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    public FlatArray<DbParameter> GetFilterParameters()
        =>
        this.BuildFilterParameters();
}
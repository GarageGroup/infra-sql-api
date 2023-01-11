using System;

namespace GGroupp.Infra;

public sealed record class DbParameterArrayFilter : IDbFilter
{
    public DbParameterArrayFilter(string fieldName, DbArrayFilterOperator @operator, FlatArray<object?> fieldValues)
    {
        FieldName = fieldName ?? string.Empty;
        Operator = @operator;
        FieldValues = fieldValues;
        ParameterPrefix = FieldName;
    }

    public DbParameterArrayFilter(string fieldName, DbArrayFilterOperator @operator, FlatArray<object?> fieldValues, string parameterPrefix)
    {
        FieldName = fieldName ?? string.Empty;
        Operator = @operator;
        FieldValues = fieldValues;
        ParameterPrefix = string.IsNullOrEmpty(parameterPrefix) ? FieldName : parameterPrefix;
    }

    public string FieldName { get; }

    public DbArrayFilterOperator Operator { get; }

    public FlatArray<object?> FieldValues { get; }

    public string ParameterPrefix { get; }

    string IDbFilter.GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    FlatArray<DbParameter> IDbFilter.GetFilterParameters()
        =>
        this.BuildFilterParameters();
}
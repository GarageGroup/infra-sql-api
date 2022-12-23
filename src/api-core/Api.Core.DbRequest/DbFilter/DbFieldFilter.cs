using System;

namespace GGroupp.Infra;

public sealed record class DbFieldFilter : IDbFilter
{
    private const string DefaultRawValue = "null";

    public DbFieldFilter(string fieldName, DbFilterOperator @operator, string rawFieldValue)
    {
        FieldName = fieldName ?? string.Empty;
        Operator = @operator;
        RawFieldValue = string.IsNullOrWhiteSpace(rawFieldValue) ? DefaultRawValue : rawFieldValue;
    }

    public string FieldName { get; }

    public DbFilterOperator Operator { get; }

    public string RawFieldValue { get; }

    string IDbFilter.GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    FlatArray<DbParameter> IDbFilter.GetFilterParameters()
        =>
        default;
}
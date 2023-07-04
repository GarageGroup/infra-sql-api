using System;

namespace GarageGroup.Infra;

public sealed record class DbFieldFilter : IDbFilter
{
    private const string DefaultRawValue = "null";

    public DbFieldFilter(string fieldName, DbFilterOperator @operator, string rawFieldValue)
    {
        FieldName = fieldName.OrEmpty();
        Operator = @operator;
        RawFieldValue = string.IsNullOrWhiteSpace(rawFieldValue) ? DefaultRawValue : rawFieldValue;
    }

    public string FieldName { get; }

    public DbFilterOperator Operator { get; }

    public string RawFieldValue { get; }

    public string GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    public FlatArray<DbParameter> GetFilterParameters()
        =>
        default;
}
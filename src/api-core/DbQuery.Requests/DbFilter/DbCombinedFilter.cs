using System;

namespace GGroupp.Infra;

public sealed record class DbCombinedFilter : IDbFilter
{
    public DbCombinedFilter(DbLogicalOperator @operator, FlatArray<IDbFilter> filters)
    {
        Operator = @operator;
        Filters = filters;
    }

    public DbLogicalOperator Operator { get; }

    public FlatArray<IDbFilter> Filters { get; }

    public string GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    public FlatArray<DbParameter> GetFilterParameters()
        =>
        this.BuildFilterParameters();
}
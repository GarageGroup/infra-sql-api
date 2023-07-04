using System;
#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace GarageGroup.Infra;

public sealed record class DbCombinedFilter : IDbFilter
{
    public DbCombinedFilter(DbLogicalOperator @operator)
        =>
        Operator = @operator;

#if NET7_0_OR_GREATER
    [SetsRequiredMembers]
#endif
    public DbCombinedFilter(DbLogicalOperator @operator, FlatArray<IDbFilter> filters)
    {
        Operator = @operator;
        Filters = filters;
    }

    public DbLogicalOperator Operator { get; }

#if NET7_0_OR_GREATER
    public required FlatArray<IDbFilter> Filters { get; init; }
#else
    public FlatArray<IDbFilter> Filters { get; init; }
#endif

    public string GetFilterSqlQuery()
        =>
        this.BuildFilterSqlQuery();

    public FlatArray<DbParameter> GetFilterParameters()
        =>
        this.BuildFilterParameters();
}
using System;

namespace GarageGroup.Infra;

public sealed record class DbIfQuery : IDbQuery
{
    public DbIfQuery(IDbFilter condition, IDbQuery thenQuery, IDbQuery? elseQuery = null)
    {
        Condition = condition;
        ThenQuery = thenQuery;
        ElseQuery = elseQuery;
    }

    public IDbFilter Condition { get; }

    public IDbQuery ThenQuery { get; }

    public IDbQuery? ElseQuery { get; }

    public int? TimeoutInSeconds { get; init; }

    public string GetSqlQuery()
        =>
        this.BuildSqlQuery();

    public FlatArray<DbParameter> GetParameters()
        =>
        this.BuildParameters();
}
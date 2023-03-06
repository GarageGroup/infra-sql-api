using System;

namespace GGroupp.Infra;

public sealed record class DbCombinedQuery : IDbQuery
{
    public DbCombinedQuery(FlatArray<IDbQuery> queries)
        =>
        Queries = queries;

    public FlatArray<IDbQuery> Queries { get; }

    public int? TimeoutInSeconds { get; init; }

    public string GetSqlQuery()
        =>
        this.BuildSqlQuery();

    public FlatArray<DbParameter> GetParameters()
        =>
        this.BuildParameters();
}
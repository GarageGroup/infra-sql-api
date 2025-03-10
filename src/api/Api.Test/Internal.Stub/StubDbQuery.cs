using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal sealed record class StubDbQuery : IDbQuery
{
    private readonly IReadOnlyDictionary<SqlDialect, string> queries;

    private readonly FlatArray<DbParameter> parameters;

    internal StubDbQuery(IReadOnlyDictionary<SqlDialect, string> queries, FlatArray<DbParameter> parameters)
    {
        this.queries = queries;
        this.parameters = parameters;
    }

    public int? TimeoutInSeconds { get; init; }

    public string GetSqlQuery(SqlDialect dialect)
        =>
        queries[dialect];

    public FlatArray<DbParameter> GetParameters()
        =>
        parameters;
}
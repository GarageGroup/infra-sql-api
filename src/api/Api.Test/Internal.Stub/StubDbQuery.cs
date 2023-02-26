using System;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

internal sealed record class StubDbQuery : IDbQuery
{
    private readonly string query;

    private readonly FlatArray<DbParameter> parameters;

    internal StubDbQuery(string query, FlatArray<DbParameter> parameters)
    {
        this.query = query;
        this.parameters = parameters;
    }

    public string GetSqlQuery()
        =>
        query;

    public FlatArray<DbParameter> GetParameters()
        =>
        parameters;
}
using System;

namespace GGroupp.Infra;

public sealed record class DbQuery : IDbQuery
{
    public DbQuery(string query)
    {
        Query = query ?? string.Empty;
        Parameters = FlatArray.Empty<DbParameter>();
    }

    public DbQuery(string query, FlatArray<DbParameter> parameters)
    {
        Query = query ?? string.Empty;
        Parameters = parameters;
    }

    public string Query { get; }

    public FlatArray<DbParameter> Parameters { get; }

    public int? TimeoutInSeconds { get; init; }

    public string GetSqlQuery()
        =>
        Query;

    public FlatArray<DbParameter> GetParameters()
        =>
        Parameters;
}
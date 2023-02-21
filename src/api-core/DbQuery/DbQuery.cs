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

    string IDbQuery.GetSqlQuery()
        =>
        Query;

    FlatArray<DbParameter> IDbQuery.GetParameters()
        =>
        Parameters;
}
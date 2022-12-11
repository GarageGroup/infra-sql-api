using System;

namespace GGroupp.Infra;

public sealed record class DbRequest
{
    public DbRequest(string query)
    {
        Query = query ?? string.Empty;
        Parameters = FlatArray.Empty<DbParameter>();
    }

    public DbRequest(string query, FlatArray<DbParameter> parameters)
    {
        Query = query ?? string.Empty;
        Parameters = parameters;
    }

    public string Query { get; }

    public FlatArray<DbParameter> Parameters { get; }
}
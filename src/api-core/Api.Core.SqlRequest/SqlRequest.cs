using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

public sealed record class SqlRequest
{
    public SqlRequest(string query)
    {
        Query = query ?? string.Empty;
        Parameters = FlatArray.Empty<KeyValuePair<string, object?>>();
    }

    public SqlRequest(string query, FlatArray<KeyValuePair<string, object?>> parameters)
    {
        Query = query ?? string.Empty;
        Parameters = parameters;
    }

    public string Query { get; }

    public FlatArray<KeyValuePair<string, object?>> Parameters { get; }
}
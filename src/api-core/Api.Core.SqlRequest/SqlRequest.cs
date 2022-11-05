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

    public SqlRequest(string query, [AllowNull] FlatArray<KeyValuePair<string, object?>> parameters = null)
    {
        Query = query ?? string.Empty;
        Parameters = parameters ?? FlatArray.Empty<KeyValuePair<string, object?>>();
    }

    public string Query { get; }

    public FlatArray<KeyValuePair<string, object?>> Parameters { get; }
}
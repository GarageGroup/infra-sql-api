using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal sealed record class StubDbCommandRequest
{
    public required string CommandText { get; init; }

    public required IReadOnlyCollection<DbParameter>? Parameters { get; init; }

    public required int? Timeout { get; init; }
}
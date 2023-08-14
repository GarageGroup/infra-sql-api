using System;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal sealed class StubException : Exception
{
    public StubException(string? message) : base(message)
    {
    }
}
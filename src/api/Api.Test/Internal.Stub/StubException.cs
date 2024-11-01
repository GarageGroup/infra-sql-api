using System;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal sealed class StubException(string? message) : Exception(message);
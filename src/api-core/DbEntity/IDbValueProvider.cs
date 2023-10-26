using System;

namespace GarageGroup.Infra;

public interface IDbValueProvider
{
    bool IsNull();

    bool GetBoolean();

    byte GetByte();

    DateTime GetDateTime();

    DateTimeOffset GetDateTimeOffset();

    DateOnly GetDateOnly();

    decimal GetDecimal();

    double GetDouble();

    float GetFloat();

    Guid GetGuid();

    short GetInt16();

    int GetInt32();

    long GetInt64();

    string? GetString();

    T Get<T>() where T : notnull;

    object Get();
}
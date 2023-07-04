using System;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal interface IStubDbDataReader
{
    int FieldCount { get; }

    bool GetBoolean(int ordinal);

    byte GetByte(int ordinal);

    DateTime GetDateTime(int ordinal);

    decimal GetDecimal(int ordinal);

    double GetDouble(int ordinal);

    float GetFloat(int ordinal);

    Guid GetGuid(int ordinal);

    short GetInt16(int ordinal);

    int GetInt32(int ordinal);

    long GetInt64(int ordinal);

    string GetName(int ordinal);

    string GetString(int ordinal);

    object GetValue(int ordinal);

    bool IsDBNull(int ordinal);

    bool Read();
}
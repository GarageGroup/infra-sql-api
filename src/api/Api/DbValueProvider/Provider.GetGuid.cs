using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public Guid GetGuid()
        =>
        dbDataReader.GetGuid(fieldIndex);
}
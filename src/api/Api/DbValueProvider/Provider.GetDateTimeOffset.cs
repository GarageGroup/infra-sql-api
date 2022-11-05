using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public DateTimeOffset GetDateTimeOffset()
        =>
        dbDataReader.GetFieldValue<DateTimeOffset>(fieldIndex);
}
using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public DateOnly GetDateOnly()
        =>
        dbDataReader.GetFieldValue<DateOnly>(fieldIndex);
}
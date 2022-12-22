using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public DateTimeOffset GetDateTimeOffset()
    {
        try
        {
            return dbDataReader.GetFieldValue<DateTimeOffset>(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
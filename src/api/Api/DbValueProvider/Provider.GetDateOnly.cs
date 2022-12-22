using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public DateOnly GetDateOnly()
    {
        try
        {
            return dbDataReader.GetFieldValue<DateOnly>(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
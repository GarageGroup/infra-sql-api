using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public T Get<T>() where T
        : notnull
    {
        try
        {
            return dbDataReader.GetFieldValue<T>(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
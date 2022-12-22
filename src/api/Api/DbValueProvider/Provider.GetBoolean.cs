using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public bool GetBoolean()
    {
        try
        {
            return dbDataReader.GetBoolean(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
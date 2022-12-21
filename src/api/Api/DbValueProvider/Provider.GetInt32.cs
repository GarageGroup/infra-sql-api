using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public int GetInt32()
    {
        try
        {
            return dbDataReader.GetInt32(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
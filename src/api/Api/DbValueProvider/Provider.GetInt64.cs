using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public long GetInt64()
    {
        try
        {
            return dbDataReader.GetInt64(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
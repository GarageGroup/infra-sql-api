using System;

namespace GarageGroup.Infra;

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
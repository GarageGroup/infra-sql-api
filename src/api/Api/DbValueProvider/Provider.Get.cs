using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public object Get()
    {
        try
        {
            return dbDataReader.GetValue(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
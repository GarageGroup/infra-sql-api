using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public float GetFloat()
    {
        try
        {
            return dbDataReader.GetFloat(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
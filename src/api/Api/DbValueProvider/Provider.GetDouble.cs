using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public double GetDouble()
    {
        try
        {
            return dbDataReader.GetDouble(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
using System;

namespace GGroupp.Infra;

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
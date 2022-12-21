using System;

namespace GGroupp.Infra;

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
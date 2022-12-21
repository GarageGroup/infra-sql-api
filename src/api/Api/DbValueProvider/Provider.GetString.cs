using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public string? GetString()
    {
        try
        {
            return dbDataReader.GetString(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
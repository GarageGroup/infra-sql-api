using System;

namespace GGroupp.Infra;

partial class DbValueProvider
{
    public DateTime GetDateTime()
    {
        try
        {
            return dbDataReader.GetDateTime(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
using System;

namespace GarageGroup.Infra;

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
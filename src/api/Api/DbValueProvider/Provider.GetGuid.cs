using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public Guid GetGuid()
    {
        try
        {
            return dbDataReader.GetGuid(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public bool IsNull()
    {
        try
        {
            return dbDataReader.IsDBNull(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
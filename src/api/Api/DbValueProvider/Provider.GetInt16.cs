using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public short GetInt16()
    {
        try
        {
            return dbDataReader.GetInt16(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public byte GetByte()
    {
        try
        {
            return dbDataReader.GetByte(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
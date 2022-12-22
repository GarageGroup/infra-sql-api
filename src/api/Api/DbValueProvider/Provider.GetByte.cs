using System;

namespace GGroupp.Infra;

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
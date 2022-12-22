using System;

namespace GGroupp.Infra;

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
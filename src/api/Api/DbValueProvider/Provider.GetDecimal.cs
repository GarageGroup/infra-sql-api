using System;

namespace GarageGroup.Infra;

partial class DbValueProvider
{
    public decimal GetDecimal()
    {
        try
        {
            return dbDataReader.GetDecimal(fieldIndex);
        }
        catch (Exception exception)
        {
            throw WrapSourceException(exception);
        }
    }
}
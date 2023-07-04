using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

partial class DbValue
{
    public byte CastToByte()
        =>
        dbValueProvider.GetByte();

    public byte? CastToNullableByte()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetByte();

    public static implicit operator byte(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToByte();
    }

    public static implicit operator byte?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableByte();
}
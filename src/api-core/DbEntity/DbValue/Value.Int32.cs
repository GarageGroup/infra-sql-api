using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

partial class DbValue
{
    public int CastToInt32()
        =>
        dbValueProvider.GetInt32();

    public int? CastToNullableInt32()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetInt32();

    public static implicit operator int(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToInt32();
    }

    public static implicit operator int?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableInt32();
}
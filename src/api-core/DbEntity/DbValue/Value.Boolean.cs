using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

partial class DbValue
{
    public bool CastToBoolean()
        =>
        dbValueProvider.GetBoolean();

    public bool? CastToNullableBoolean()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetBoolean();

    public static implicit operator bool(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToBoolean();
    }

    public static implicit operator bool?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableBoolean();
}
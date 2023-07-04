using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

partial class DbValue
{
    public float CastToFloat()
        =>
        dbValueProvider.GetFloat();

    public float? CastToNullableFloat()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetFloat();

    public static implicit operator float(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToFloat();
    }

    public static implicit operator float?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableFloat();
}
using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public double CastToDouble()
        =>
        dbValueProvider.GetDouble();

    public double? CastToNullableDouble()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetDouble();

    public static implicit operator double(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToDouble();
    }

    public static implicit operator double?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableDouble();
}
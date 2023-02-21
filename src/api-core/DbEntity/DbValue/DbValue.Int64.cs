using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public long CastToInt64()
        =>
        dbValueProvider.GetInt64();

    public long? CastToNullableInt64()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetInt64();

    public static implicit operator long(DbValue dbValue)
    {
        _ = dbValue ?? throw new ArgumentNullException(nameof(dbValue));
        return dbValue.CastToInt64();
    }

    public static implicit operator long?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableInt64();
}
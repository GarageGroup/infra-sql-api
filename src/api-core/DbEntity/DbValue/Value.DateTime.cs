using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public DateTime CastToDateTime()
        =>
        dbValueProvider.GetDateTime();

    public DateTime? CastToNullableDateTime()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetDateTime();

    public static implicit operator DateTime(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToDateTime();
    }

    public static implicit operator DateTime?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableDateTime();
}
using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public DateOnly CastToDateOnly()
        =>
        dbValueProvider.GetDateOnly();

    public DateOnly? CastToNullableDateOnly()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetDateOnly();

    public static implicit operator DateOnly(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToDateOnly();
    }

    public static implicit operator DateOnly?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableDateOnly();
}
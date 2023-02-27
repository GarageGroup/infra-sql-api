using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public DateTimeOffset CastToDateTimeOffset()
        =>
        dbValueProvider.GetDateTimeOffset();

    public DateTimeOffset? CastToNullableDateTimeOffset()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetDateTimeOffset();

    public static implicit operator DateTimeOffset(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToDateTimeOffset();
    }

    public static implicit operator DateTimeOffset?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableDateTimeOffset();
}
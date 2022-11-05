using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public Guid CastToGuid()
        =>
        dbValueProvider.GetGuid();

    public Guid? CastToNullableGuid()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetGuid();

    public static implicit operator Guid(DbValue dbValue)
    {
        _ = dbValue ?? throw new ArgumentNullException(nameof(dbValue));
        return dbValue.CastToGuid();
    }

    public static implicit operator Guid?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableGuid();
}
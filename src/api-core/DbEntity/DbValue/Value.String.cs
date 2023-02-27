using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public string CastToString()
        =>
        dbValueProvider.GetString() ?? string.Empty;

    public string? CastToNullableString()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetString();

    public static implicit operator string?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableString();
}
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

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
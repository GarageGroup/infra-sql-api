using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public string? CastToString()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetString();

    public static implicit operator string?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToString();
}
namespace GarageGroup.Infra;

partial class DbValue
{
    public object CastToObject()
        =>
        dbValueProvider.Get();

    public object? CastToNullableObject()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.Get();
}
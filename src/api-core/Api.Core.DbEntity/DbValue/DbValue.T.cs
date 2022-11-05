namespace GGroupp.Infra;

partial class DbValue
{
    public T? CastTo<T>()
        where T : notnull
        =>
        dbValueProvider.Get<T>();

    public T? CastToNullable<T>()
        where T : struct
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.Get<T>();
}
﻿namespace GarageGroup.Infra;

partial class DbValue
{
    public T CastTo<T>()
        where T : notnull
        =>
        dbValueProvider.Get<T>();

    public T? CastToNullable<T>()
        where T : class
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.Get<T>();

    public T? CastToNullableStruct<T>()
        where T : struct
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.Get<T>();
}
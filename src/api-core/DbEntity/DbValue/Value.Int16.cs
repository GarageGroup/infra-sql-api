﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

partial class DbValue
{
    public short CastToInt16()
        =>
        dbValueProvider.GetInt16();

    public short? CastToNullableInt16()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetInt16();

    public static implicit operator short(DbValue dbValue)
    {
        ArgumentNullException.ThrowIfNull(dbValue);
        return dbValue.CastToInt16();
    }

    public static implicit operator short?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableInt16();
}
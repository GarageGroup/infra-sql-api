﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace GGroupp.Infra;

partial class DbValue
{
    public decimal CastToDecimal()
        =>
        dbValueProvider.GetDecimal();

    public decimal? CastToNullableDecimal()
        =>
        dbValueProvider.IsNull() ? null : dbValueProvider.GetDecimal();

    public static implicit operator decimal(DbValue dbValue)
    {
        _ = dbValue ?? throw new ArgumentNullException(nameof(dbValue));
        return dbValue.CastToDecimal();
    }

    public static implicit operator decimal?([AllowNull] DbValue dbValue)
        =>
        dbValue?.CastToNullableDecimal();
}
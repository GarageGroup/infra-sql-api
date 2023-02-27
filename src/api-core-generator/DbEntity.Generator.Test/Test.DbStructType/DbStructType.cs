using System;
using GGroupp.Infra;
using PrimeFuncPack.UnitTest;

namespace GGroupp.TestType;

[DbEntity]
internal readonly struct DbStructType
{
    [DbField]
    public short Id { get; init; }

    [DbField]
    public bool? IsActual { get; init; }

    [DbField]
    internal DateTimeOffset CreateAt { get; init; }

    [DbField]
    public DateOnly? ProductDate { get; init; }

    [DbField("c.Count")]
    public int? TotalCount { get; init; }

    [DbField]
    public decimal Price { get; init; }

    [DbField]
    public RefType AddionalData { get; init; }
}
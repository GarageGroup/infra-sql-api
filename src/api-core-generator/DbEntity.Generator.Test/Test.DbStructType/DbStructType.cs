using System;
using GarageGroup.Infra;
using PrimeFuncPack.UnitTest;

namespace GarageGroup.TestType;

[DbEntity]
internal readonly partial struct DbStructType
{
    [DbSelect("QueryAll")]
    public short Id { get; init; }

    [DbSelect("QueryAll")]
    public bool? IsActual { get; init; }

    [DbSelect("QueryAll")]
    internal DateTimeOffset CreateAt { get; init; }

    [DbSelect("QueryAll")]
    public DateOnly? ProductDate { get; init; }

    [DbSelect("QueryTotalCount", FieldName = "COUNT(*)")]
    public int? TotalCount { get; init; }

    [DbSelect("QueryAll")]
    public decimal Price { get; init; }

    [DbSelect("QueryAll")]
    public RefType AddionalData { get; init; }
}
using System;
using System.Collections.Generic;
using GarageGroup.Infra;
using PrimeFuncPack.UnitTest;

namespace GarageGroup.TestType;

[DbEntity("Product", "p")]
[DbJoin(DbJoinType.Right, "Right", "r", "r.Id = p.RightId")]
[DbJoin(DbJoinType.Left, "Left", "l", "l.Id = p.LeftId")]
internal readonly partial record struct DbStructRecord
{
    [DbSelect("QueryAll"), DbSelect("QueryLeft"), DbSelect("QueryRight")]
    public byte Id { get; init; }

    [DbSelect("QueryAll", "p"), DbSelect("QueryLeft", "p"), DbSelect("QueryRight", "p")]
    public bool IsActive { get; init; }

    [DbSelect("QueryAll", "l"), DbSelect("QueryLeft", "l", GroupBy = true)]
    public DateOnly Date { get; init; }

    [DbSelect("QueryAll", "r"), DbSelect("QueryRight", "r")]
    public DateTime? ModifiedAt { get; init; }

    [DbSelect("QueryAll", FieldName = "c.Price", GroupBy = true)]
    public decimal? Price { get; init; }

    [DbSelect("QueryAll", "p", GroupBy = true)]
    public short? Sum { get; init; }

    [DbSelect("QueryAll", "p")]
    public string Name { get; init; }

    [DbSelect("QueryAll", "p")]
    public RecordStruct? AdditionalData { get; init; }

    [DbExtensionData]
    public IReadOnlyDictionary<string, object?> OtherFields { get; init; }
}
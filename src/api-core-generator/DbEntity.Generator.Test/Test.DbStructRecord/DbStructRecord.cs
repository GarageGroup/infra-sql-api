using System;
using GarageGroup.Infra;
using PrimeFuncPack.UnitTest;

namespace GarageGroup.TestType;

[DbEntity("Product", "p")]
[DbJoin(DbJoinType.Left, "Left", "l", "l.Id = p.LeftId")]
[DbJoin(DbJoinType.Right, "Right", "r", "r.Id = p.RightId")]
internal readonly partial record struct DbStructRecord
{
    [DbSelect("QueryAll"), DbSelect("QueryLeft"), DbSelect("QueryRight")]
    public byte Id { get; init; }

    [DbSelect("QueryAll", "p"), DbSelect("QueryLeft", "p"), DbSelect("QueryRight", "p")]
    public bool IsActive { get; init; }

    [DbSelect("QueryAll", "l"), DbSelect("QueryLeft", "l")]
    public DateOnly Date { get; init; }

    [DbSelect("QueryAll", "r"), DbSelect("QueryRight", "r")]
    public DateTime? ModifiedAt { get; init; }

    [DbSelect("QueryAll", FieldName = "c.Price")]
    public decimal? Price { get; init; }

    [DbSelect("QueryAll", "p")]
    public short? Sum { get; init; }

    [DbSelect("QueryAll", "p")]
    public string Name { get; init; }

    [DbSelect("QueryAll", "p")]
    public RecordStruct? AdditionalData { get; init; }
}
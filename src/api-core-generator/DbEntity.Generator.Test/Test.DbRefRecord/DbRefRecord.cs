using System;
using GGroupp.Infra;
using PrimeFuncPack.UnitTest;

namespace GGroupp.TestType;

[DbEntity]
public sealed record class DbRefRecord
{
    [DbField("GG_Id")]
    public long Id { get; init; }

    [DbField]
    public byte? Byte { get; init; }

    [DbField]
    public DateTime EntityTime { get; init; }

    [DbField]
    public DateTimeOffset? UpdatedAt { get; init; }

    [DbField]
    public double? Price { get; init; }

    [DbField]
    public float Sum { get; init; }

    [DbField]
    public StructType AdditionalStructData { get; init; }

    [DbField]
    public RecordType? AdditionalRefData { get; init; }
}
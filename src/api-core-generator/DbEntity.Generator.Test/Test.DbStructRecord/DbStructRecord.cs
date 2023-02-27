using System;
using GGroupp.Infra;
using PrimeFuncPack.UnitTest;

namespace GGroupp.TestType;

[DbEntity]
internal readonly partial record struct DbStructRecord
{
    [DbField]
    public byte Id { get; init; }

    [DbField]
    public bool IsActive { get; init; }

    [DbField]
    public DateOnly Date { get; init; }

    [DbField]
    public DateTime? ModifiedAt { get; init; }

    [DbField]
    public decimal? Price { get; init; }

    [DbField]
    public short? Sum { get; init; }

    [DbField]
    public string Name { get; init; }

    [DbField]
    public RecordStruct? AdditionalData { get; init; }
}
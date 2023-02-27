using System;
using GGroupp.Infra;
using PrimeFuncPack.UnitTest;

namespace GGroupp.TestType;

[DbEntity]
internal sealed class DbRefType
{
    [DbField]
    public int Id { get; init; }

    [DbField(TestData.EmptyString)]
    public Guid CrmId { get; init; }

    [DbField("p.CrmId")]
    public Guid? PropertyCrmId { get; init; }

    [DbField]
    public string? Name { get; init; }

    [DbField]
    public double Price { get; init; }

    [DbField]
    public float? Sum { get; init; }

    [DbField]
    public long? ExternalId { get; init; }

    public int SkippedValue { get; init; }
}
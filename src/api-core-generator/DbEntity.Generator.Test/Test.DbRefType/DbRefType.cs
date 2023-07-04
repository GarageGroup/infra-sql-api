using System;
using GarageGroup.Infra;

namespace GarageGroup.TestType;

[DbEntity]
internal sealed partial class DbRefType
{
    public int Id { get; init; }

    public Guid CrmId { get; init; }

    public Guid? PropertyCrmId { get; init; }

    public string? Name { get; init; }

    public double Price { get; init; }

    public float? Sum { get; init; }

    public long? ExternalId { get; init; }

    [DbFieldIgnore]
    public int SkippedValue { get; init; }
}
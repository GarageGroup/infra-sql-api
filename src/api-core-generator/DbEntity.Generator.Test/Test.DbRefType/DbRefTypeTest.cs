using System;
using System.Collections.Generic;
using DeepEqual.Syntax;
using GarageGroup.TestType;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

public static class DbRefTypeTest
{
    [Fact]
    public static void ReadEntity_DbItemIsNull_ExpectArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbItem", ex.ParamName);

        static void Test()
            =>
            _ = DbRefType.ReadEntity(null!);
    }

    [Fact]
    public static void ReadEntity_DbItemIsNotNull_ExpectCorrectInitializedEntity()
    {
        var orThrowValues = new Dictionary<string, DbValue>
        {
            ["Id"] = StubDbValue.CreateInt32(TestData.One),
            ["CrmId"] = StubDbValue.CreateGuid(Guid.Parse("a1a60dd1-0357-46ee-939f-402f6f344dd8")),
            ["Price"] = StubDbValue.CreateDouble(754.951)
        };

        var orDefaultValues = new Dictionary<string, DbValue?>
        {
            ["PropertyCrmId"] = StubDbValue.CreateNullableGuid(Guid.Parse("4a0e47b9-0e8c-4bcc-96d6-e2f5263f8a0a")),
            ["Name"] = StubDbValue.CreateNullableString(TestData.SomeString),
            ["Sum"] = StubDbValue.CreateNullableFloat(-17895),
            ["ExternalId"] = StubDbValue.CreateNullableInt64(12391298712)
        };

        var dbItem = StubDbItem.Create(orThrowValues, orDefaultValues);
        var actual = DbRefType.ReadEntity(dbItem);

        var expected = new DbRefType
        {
            Id = TestData.One,
            CrmId = Guid.Parse("a1a60dd1-0357-46ee-939f-402f6f344dd8"),
            PropertyCrmId = Guid.Parse("4a0e47b9-0e8c-4bcc-96d6-e2f5263f8a0a"),
            Name = TestData.SomeString,
            Price = 754.951,
            Sum = -17895,
            ExternalId = 12391298712,
            FieldValues = new(0)
        };

        actual.ShouldDeepEqual(expected);
    }
}
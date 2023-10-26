using System;
using System.Collections.Generic;
using GarageGroup.TestType;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

public static class DbRefRecordTest
{
    [Fact]
    public static void ReadEntity_DbItemIsNull_ExpectArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbItem", ex.ParamName);

        static void Test()
            =>
            _ = DbRefRecord.ReadEntity(null!);
    }

    [Fact]
    public static void ReadEntity_DbItemIsNotNull_ExpectCorrectInitializedEntity()
    {
        var orThrowValues = new Dictionary<string, DbValue>
        {
            ["Id"] = StubDbValue.CreateInt64(-780119823712),
            ["EntityTime"] = StubDbValue.CreateDateTime(new(2023, 02, 25, 11, 47, 01)),
            ["Sum"] = StubDbValue.CreateFloat(134.45E-2f),
            ["AdditionalStructData"] = StubDbValue.Create(TestData.SomeTextStructType)
        };

        var orDefaultValues = new Dictionary<string, DbValue?>
        {
            ["Byte"] = StubDbValue.CreateNullableByte(215),
            ["UpdatedAt"] = StubDbValue.CreateNullableDateTimeOffset(new(1997, 12, 23, 15, 07, 59, TimeSpan.FromHours(-1))),
            ["Price"] = StubDbValue.CreateNullableDouble(-20170.79),
            ["AdditionalRefData"] = StubDbValue.CreateNullable(TestData.MinusFifteenIdNullNameRecord)
        };

        var dbItem = StubDbItem.Create(orThrowValues, orDefaultValues);
        var actual = DbRefRecord.ReadEntity(dbItem);

        var expected = new DbRefRecord
        {
            Id = -780119823712,
            Byte = 215,
            EntityTime = new(2023, 02, 25, 11, 47, 01),
            UpdatedAt = new(1997, 12, 23, 15, 07, 59, TimeSpan.FromHours(-1)),
            Price = -20170.79,
            Sum = 134.45E-2f,
            AdditionalStructData = TestData.SomeTextStructType,
            AdditionalRefData = TestData.MinusFifteenIdNullNameRecord
        };

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void GetQueryAll_ExpectCorrectQuery()
    {
        var actual = DbRefRecord.QueryAll;

        var expected = new DbSelectQuery("Product", "p")
        {
            JoinedTables = new DbJoinedTable(DbJoinType.Inner, "Unit", "u", new DbRawFilter("u.Id = p.UnitId")).AsFlatArray(),
            SelectedFields = new("Id", "Byte", "Time AS EntityTime", "UpdatedAt", "Product.Price", "u.Sum", "AdditionalRefData"),
            GroupByFields = new("Id")
        };

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static void GetQueryId_ExpectCorrectQuery()
    {
        var actual = DbRefRecord.QueryId;

        var expected = new DbSelectQuery("Product", "p")
        {
            SelectedFields = new("p.Id AS Id")
        };

        Assert.StrictEqual(expected, actual);
    }
}
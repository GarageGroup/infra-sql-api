using System;
using System.Collections.Generic;
using DeepEqual.Syntax;
using GarageGroup.TestType;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.DbEntity.Generator.Test;

public static class DbStructRecordTest
{
    [Fact]
    public static void ReadEntity_DbItemIsNull_ExpectArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbItem", ex.ParamName);

        static void Test()
            =>
            _ = DbStructRecord.ReadEntity(null!);
    }

    [Fact]
    public static void ReadEntity_DbItemIsNotNull_ExpectCorrectInitializedEntity()
    {
        var orThrowValues = new Dictionary<string, DbValue>
        {
            ["Id"] = StubDbValue.CreateByte(93),
            ["IsActive"] = StubDbValue.CreateBoolean(true),
            ["Date"] = StubDbValue.CreateDateOnly(new(2006, 07, 03)),
            ["Name"] = StubDbValue.CreateString(TestData.MixedWhiteSpacesString)
        };

        var orDefaultValues = new Dictionary<string, DbValue?>
        {
            ["ModifiedAt"] = StubDbValue.CreateNullableDateTime(new(2015, 11, 24, 12, 36, 41)),
            ["Price"] = StubDbValue.CreateNullableDecimal(5610.7m),
            ["Sum"] = StubDbValue.CreateNullableInt16(-2750),
            ["AdditionalData"] = StubDbValue.CreateNullableStruct<RecordStruct>(TestData.SomeTextRecordStruct),
            ["name"] = StubDbValue.CreateString("Some text value"),
            ["OtherSecond"] = StubDbValue.CreateNullable<object>(null),
            ["OtherThird"] = StubDbValue.CreateNullable(TestData.PlusFifteenIdLowerSomeStringNameRecord)
        };

        var dbItem = StubDbItem.Create(orThrowValues, orDefaultValues);
        var actual = DbStructRecord.ReadEntity(dbItem);

        var expected = new DbStructRecord
        {
            Id = 93,
            IsActive = true,
            Date = new(2006, 07, 03),
            ModifiedAt = new(2015, 11, 24, 12, 36, 41),
            Price = 5610.7m,
            Sum = -2750,
            Name = TestData.MixedWhiteSpacesString,
            AdditionalData = TestData.SomeTextRecordStruct,
            OtherFields = new Dictionary<string, object?>
            {
                ["name"] = "Some text value",
                ["OtherSecond"] = null,
                ["OtherThird"] = TestData.PlusFifteenIdLowerSomeStringNameRecord
            }
        };

        actual.ShouldDeepEqual(expected);
    }

    [Fact]
    public static void GetQueryAll_ExpectCorrectQuery()
    {
        var actual = DbStructRecord.QueryAll;

        var expected = new DbSelectQuery("Product", "p")
        {
            JoinedTables = new DbJoinedTable[]
            {
                new(DbJoinType.Right, "Right", "r", new DbRawFilter("r.Id = p.RightId")),
                new(DbJoinType.Left, "Left", "l", new DbRawFilter("l.Id = p.LeftId"))
            },
            SelectedFields = new("Id", "p.IsActive", "l.Date", "r.ModifiedAt", "c.Price AS Price", "p.Sum", "p.Name", "p.AdditionalData"),
            GroupByFields = new("c.Price", "p.Sum")
        };

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static void GetQueryLeft_ExpectCorrectQuery()
    {
        var actual = DbStructRecord.QueryLeft;

        var expected = new DbSelectQuery("Product", "p")
        {
            JoinedTables = new DbJoinedTable[]
            {
                new(DbJoinType.Left, "Left", "l", new DbRawFilter("l.Id = p.LeftId"))
            },
            SelectedFields = new("Id", "p.IsActive", "l.Date"),
            GroupByFields = new("l.Date")
        };

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static void GetQueryRight_ExpectCorrectQuery()
    {
        var actual = DbStructRecord.QueryRight;

        var expected = new DbSelectQuery("Product", "p")
        {
            JoinedTables = new DbJoinedTable[]
            {
                new(DbJoinType.Right, "Right", "r", new DbRawFilter("r.Id = p.RightId"))
            },
            SelectedFields = new("Id", "p.IsActive", "r.ModifiedAt")
        };

        Assert.StrictEqual(expected, actual);
    }
}
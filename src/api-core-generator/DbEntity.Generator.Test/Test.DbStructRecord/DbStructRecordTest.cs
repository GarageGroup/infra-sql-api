using System;
using System.Collections.Generic;
using GGroupp.TestType;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.DbEntity.Generator.Test;

public static class DbStructRecordTest
{
    [Fact]
    public static void EntityFrom_DbItemIsNull_ExpectArgumentNullException()
    {
        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbItem", ex.ParamName);

        static void Test()
            =>
            _ = DbStructRecordEntity.From(null!);
    }

    [Fact]
    public static void EntityFrom_DbItemIsNotNull_ExpectCorrectInitializedEntity()
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
            ["AdditionalData"] = StubDbValue.CreateNullableStruct<RecordStruct>(TestData.SomeTextRecordStruct)
        };

        var dbItem = StubDbItem.Create(orThrowValues, orDefaultValues);
        var actual = DbStructRecordEntity.From(dbItem);

        var expected = new DbStructRecord
        {
            Id = 93,
            IsActive = true,
            Date = new(2006, 07, 03),
            ModifiedAt = new(2015, 11, 24, 12, 36, 41),
            Price = 5610.7m,
            Sum = -2750,
            Name = TestData.MixedWhiteSpacesString,
            AdditionalData = TestData.SomeTextRecordStruct
        };

        Assert.Equal(expected, actual);
    }
}
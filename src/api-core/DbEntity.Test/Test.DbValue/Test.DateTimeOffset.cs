using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToDateTimeOffset_ExpectDateTimeOffsetValueFromProvider()
    {
        var value = new DateTimeOffset(2016, 11, 24, 05, 08, 43, TimeSpan.FromHours(3));
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDateTimeOffset() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToDateTimeOffset();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableDateTimeOffset_IsNullReturnsTrue_ExpectNull()
    {
        var value = new DateTimeOffset(2012, 12, 01, 23, 15, 26, TimeSpan.FromMinutes(-90));
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDateTimeOffset() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDateTimeOffset();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableDateTimeOffset_IsNullReturnsFalse_ExpectDateTimeOffsetValueFromProvider()
    {
        var value = new DateTimeOffset(2023, 02, 20, 16, 01, 59, default);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateTimeOffset() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDateTimeOffset();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToDateTimeOffset_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (DateTimeOffset)dbValue;
    }

    [Fact]
    public static void ImplicitToDateTimeOffset_DbValueIsNotNull_ExpectDateTimeOffsetValueFromProvider()
    {
        var value = new DateTimeOffset(2021, 12, 15, 22, 08, 00, TimeSpan.FromHours(-6));
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDateTimeOffset() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateTimeOffset actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableDateTimeOffset_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        DateTimeOffset? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDateTimeOffset_IsNullReturnsTrue_ExpectNull()
    {
        var value = new DateTimeOffset(2020, 03, 06, 18, 11, 45, TimeSpan.FromHours(-3));
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDateTimeOffset() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateTimeOffset? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDateTimeOffset_IsNullReturnsFalse_ExpectDateTimeOffsetValueFromProvider()
    {
        var value = new DateTimeOffset(2019, 05, 30, 02, 07, 25, TimeSpan.FromHours(-11));
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateTimeOffset() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateTimeOffset? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
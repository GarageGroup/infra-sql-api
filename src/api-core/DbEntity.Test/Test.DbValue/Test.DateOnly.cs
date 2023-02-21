using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToDateOnly_ExpectDateOnlyValueFromProvider()
    {
        var value = new DateOnly(2022, 01, 15);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDateOnly() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToDateOnly();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableDateOnly_IsNullReturnsTrue_ExpectNull()
    {
        var value = new DateOnly(2023, 02, 01);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDateOnly() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDateOnly();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableDateOnly_IsNullReturnsFalse_ExpectDateOnlyValueFromProvider()
    {
        var value = new DateOnly(2019, 12, 11);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateOnly() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDateOnly();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToDateOnly_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (DateOnly)dbValue;
    }

    [Fact]
    public static void ImplicitToDateOnly_DbValueIsNotNull_ExpectDateOnlyValueFromProvider()
    {
        var value = new DateOnly(2021, 10, 26);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDateOnly() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateOnly actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableDateOnly_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        DateOnly? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDateOnly_IsNullReturnsTrue_ExpectNull()
    {
        var value = new DateOnly(2020, 05, 31);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDateOnly() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateOnly? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDateOnly_IsNullReturnsFalse_ExpectDateOnlyValueFromProvider()
    {
        var value = new DateOnly(2022, 06, 12);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateOnly() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateOnly? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
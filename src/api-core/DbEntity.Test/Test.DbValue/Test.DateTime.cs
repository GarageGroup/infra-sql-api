using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToDateTime_ExpectDateTimeValueFromProvider()
    {
        var value = new DateTime(2023, 02, 10, 15, 11, 21);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDateTime() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToDateTime();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableDateTime_IsNullReturnsTrue_ExpectNull()
    {
        var value = new DateTime(2018, 06, 18, 20, 30, 51);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDateTime() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDateTime();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableDateTime_IsNullReturnsFalse_ExpectDateTimeValueFromProvider()
    {
        var value = new DateTime(2016, 08, 10, 15, 17, 45);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateTime() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDateTime();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToDateTime_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (DateTime)dbValue;
    }

    [Fact]
    public static void ImplicitToDateTime_DbValueIsNotNull_ExpectDateTimeValueFromProvider()
    {
        var value = new DateTime(2022, 01, 02, 05, 59, 41);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDateTime() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateTime actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableDateTime_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        DateTime? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDateTime_IsNullReturnsTrue_ExpectNull()
    {
        var value = new DateTime(2020, 12, 01, 00, 20, 55);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDateTime() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateTime? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDateTime_IsNullReturnsFalse_ExpectDateTimeValueFromProvider()
    {
        var value = new DateTime(2023, 01, 05, 16, 00, 41);
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDateTime() == value);

        var dbValue = new DbValue(dbValueProvider);
        DateTime? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
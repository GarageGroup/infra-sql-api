using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void CastToBoolean_ExpectBooleanValueFromProvider(bool value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetBoolean() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToBoolean();

        Assert.StrictEqual(value, actual);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void CastToNullableBoolean_IsNullReturnsTrue_ExpectNull(bool value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetBoolean() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableBoolean();

        Assert.Null(actual);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void CastToNullableBoolean_IsNullReturnsFalse_ExpectBooleanValueFromProvider(bool value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(
            db => db.IsNull() == false && db.GetBoolean() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableBoolean();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToBoolean_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (bool)dbValue;
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void ImplicitToBoolean_DbValueIsNotNull_ExpectBooleanValueFromProvider(bool value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetBoolean() == value);

        var dbValue = new DbValue(dbValueProvider);
        bool actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableBoolean_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        bool? actual = dbValue;

        Assert.Null(actual);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void ImplicitToNullableBoolean_IsNullReturnsTrue_ExpectNull(bool value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetBoolean() == value);

        var dbValue = new DbValue(dbValueProvider);
        bool? actual = dbValue;

        Assert.Null(actual);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void ImplicitToNullableBoolean_IsNullReturnsFalse_ExpectBooleanValueFromProvider(bool value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(
            db => db.IsNull() == false && db.GetBoolean() == value);

        var dbValue = new DbValue(dbValueProvider);
        bool? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
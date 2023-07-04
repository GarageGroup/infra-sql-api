using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToDecimal_ExpectDecimalValueFromProvider()
    {
        const decimal value = decimal.One;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDecimal() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToDecimal();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableDecimal_IsNullReturnsTrue_ExpectNull()
    {
        const decimal value = decimal.MaxValue;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDecimal() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDecimal();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableDecimal_IsNullReturnsFalse_ExpectDecimalValueFromProvider()
    {
        const decimal value = decimal.MinusOne;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDecimal() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDecimal();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToDecimal_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (decimal)dbValue;
    }

    [Fact]
    public static void ImplicitToDecimal_DbValueIsNotNull_ExpectDecimalValueFromProvider()
    {
        const decimal value = -1231.85m;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDecimal() == value);

        var dbValue = new DbValue(dbValueProvider);
        decimal actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableDecimal_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        decimal? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDecimal_IsNullReturnsTrue_ExpectNull()
    {
        const decimal value = 251.7m;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDecimal() == value);

        var dbValue = new DbValue(dbValueProvider);
        decimal? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDecimal_IsNullReturnsFalse_ExpectDecimalValueFromProvider()
    {
        const decimal value = 90.55m;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDecimal() == value);

        var dbValue = new DbValue(dbValueProvider);
        decimal? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
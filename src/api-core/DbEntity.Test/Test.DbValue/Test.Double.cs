using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToDouble_ExpectDoubleValueFromProvider()
    {
        const double value = 1294.123;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDouble() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToDouble();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableDouble_IsNullReturnsTrue_ExpectNull()
    {
        const double value = -124.7;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDouble() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDouble();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableDouble_IsNullReturnsFalse_ExpectDoubleValueFromProvider()
    {
        const double value = 101.8;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDouble() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableDouble();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToDouble_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (double)dbValue;
    }

    [Fact]
    public static void ImplicitToDouble_DbValueIsNotNull_ExpectDoubleValueFromProvider()
    {
        const double value = 24.76;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetDouble() == value);

        var dbValue = new DbValue(dbValueProvider);
        double actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableDouble_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        double? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDouble_IsNullReturnsTrue_ExpectNull()
    {
        const double value = 92.11;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetDouble() == value);

        var dbValue = new DbValue(dbValueProvider);
        double? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableDouble_IsNullReturnsFalse_ExpectDoubleValueFromProvider()
    {
        const double value = double.MinValue;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetDouble() == value);

        var dbValue = new DbValue(dbValueProvider);
        double? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
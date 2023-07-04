using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToInt16_ExpectInt16ValueFromProvider()
    {
        const short value = -1287;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetInt16() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToInt16();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableInt16_IsNullReturnsTrue_ExpectNull()
    {
        const short value = short.MinValue;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetInt16() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableInt16();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableInt16_IsNullReturnsFalse_ExpectInt16ValueFromProvider()
    {
        const short value = short.MaxValue;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt16() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableInt16();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToInt16_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (short)dbValue;
    }

    [Fact]
    public static void ImplicitToInt16_DbValueIsNotNull_ExpectInt16ValueFromProvider()
    {
        const short value = 15;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetInt16() == value);

        var dbValue = new DbValue(dbValueProvider);
        short actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableInt16_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        short? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableInt16_IsNullReturnsTrue_ExpectNull()
    {
        const short value = -27;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetInt16() == value);

        var dbValue = new DbValue(dbValueProvider);
        short? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableInt16_IsNullReturnsFalse_ExpectInt16ValueFromProvider()
    {
        const short value = 1055;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt16() == value);

        var dbValue = new DbValue(dbValueProvider);
        short? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
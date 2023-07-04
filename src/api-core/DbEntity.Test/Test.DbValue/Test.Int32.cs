using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToInt32_ExpectInt32ValueFromProvider()
    {
        const int value = 1247;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetInt32() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToInt32();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableInt32_IsNullReturnsTrue_ExpectNull()
    {
        const int value = -12486127;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetInt32() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableInt32();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableInt32_IsNullReturnsFalse_ExpectInt32ValueFromProvider()
    {
        const int value = 123612;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt32() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableInt32();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToInt32_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (int)dbValue;
    }

    [Fact]
    public static void ImplicitToInt32_DbValueIsNotNull_ExpectInt32ValueFromProvider()
    {
        const int value = 123115;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetInt32() == value);

        var dbValue = new DbValue(dbValueProvider);
        int actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableInt32_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        int? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableInt32_IsNullReturnsTrue_ExpectNull()
    {
        const int value = int.MinValue;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetInt32() == value);

        var dbValue = new DbValue(dbValueProvider);
        int? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableInt32_IsNullReturnsFalse_ExpectInt32ValueFromProvider()
    {
        const int value = int.MaxValue;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt32() == value);

        var dbValue = new DbValue(dbValueProvider);
        int? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
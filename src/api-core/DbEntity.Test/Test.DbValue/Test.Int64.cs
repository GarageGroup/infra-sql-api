using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToInt64_ExpectInt64ValueFromProvider()
    {
        const long value = -87126487;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetInt64() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToInt64();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableInt64_IsNullReturnsTrue_ExpectNull()
    {
        const long value = 7125376;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetInt64() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableInt64();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableInt64_IsNullReturnsFalse_ExpectInt64ValueFromProvider()
    {
        const long value = long.MinValue;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt64() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableInt64();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToInt64_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (long)dbValue;
    }

    [Fact]
    public static void ImplicitToInt64_DbValueIsNotNull_ExpectInt64ValueFromProvider()
    {
        const long value = 11241;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetInt64() == value);

        var dbValue = new DbValue(dbValueProvider);
        long actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableInt64_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        long? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableInt64_IsNullReturnsTrue_ExpectNull()
    {
        const long value = -11;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetInt64() == value);

        var dbValue = new DbValue(dbValueProvider);
        long? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableInt64_IsNullReturnsFalse_ExpectInt64ValueFromProvider()
    {
        const long value = 8907512;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetInt64() == value);

        var dbValue = new DbValue(dbValueProvider);
        long? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
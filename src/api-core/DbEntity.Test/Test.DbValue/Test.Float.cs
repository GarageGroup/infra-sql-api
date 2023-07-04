using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToFloat_ExpectFloatValueFromProvider()
    {
        const float value = float.PositiveInfinity;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetFloat() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToFloat();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableFloat_IsNullReturnsTrue_ExpectNull()
    {
        const float value = -91;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetFloat() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableFloat();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableFloat_IsNullReturnsFalse_ExpectFloatValueFromProvider()
    {
        const float value = float.Epsilon;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetFloat() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableFloat();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToFloat_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (float)dbValue;
    }

    [Fact]
    public static void ImplicitToFloat_DbValueIsNotNull_ExpectFloatValueFromProvider()
    {
        const float value = -9285;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetFloat() == value);

        var dbValue = new DbValue(dbValueProvider);
        float actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableFloat_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        float? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableFloat_IsNullReturnsTrue_ExpectNull()
    {
        const float value = float.MaxValue;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetFloat() == value);

        var dbValue = new DbValue(dbValueProvider);
        float? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableFloat_IsNullReturnsFalse_ExpectFloatValueFromProvider()
    {
        const float value = 12415;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetFloat() == value);

        var dbValue = new DbValue(dbValueProvider);
        float? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
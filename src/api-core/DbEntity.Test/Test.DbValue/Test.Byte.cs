using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToByte_ExpectByteValueFromProvider()
    {
        const byte value = 27;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetByte() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToByte();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableByte_IsNullReturnsTrue_ExpectNull()
    {
        const byte value = 151;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetByte() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableByte();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableByte_IsNullReturnsFalse_ExpectByteValueFromProvider()
    {
        const byte value = 73;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetByte() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableByte();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToByte_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (byte)dbValue;
    }

    [Fact]
    public static void ImplicitToByte_DbValueIsNotNull_ExpectByteValueFromProvider()
    {
        const byte value = 12;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetByte() == value);

        var dbValue = new DbValue(dbValueProvider);
        byte actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableByte_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        byte? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableByte_IsNullReturnsTrue_ExpectNull()
    {
        const byte value = 255;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetByte() == value);

        var dbValue = new DbValue(dbValueProvider);
        byte? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableByte_IsNullReturnsFalse_ExpectByteValueFromProvider()
    {
        const byte value = 65;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetByte() == value);

        var dbValue = new DbValue(dbValueProvider);
        byte? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
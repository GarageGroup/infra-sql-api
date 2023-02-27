using Moq;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastTo_ExpectValueFromProvider()
    {
        var value = TestData.MinusFifteenIdNullNameRecord;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.Get<RecordType>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastTo<RecordType>();

        Assert.Equal(value, actual);
    }

    [Fact]
    public static void CastToNullable_IsNullReturnsTrue_ExpectNull()
    {
        var value = TestData.PlusFifteenIdRefType;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.Get<RefType>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullable<RefType>();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullable_IsNullReturnsFalse_ExpectValueFromProvider()
    {
        var value = TestData.MinusFifteenIdRefType;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.Get<RefType>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullable<RefType>();

        Assert.Equal(value, actual);
    }

    [Fact]
    public static void CastToNullableStruct_IsNullReturnsTrue_ExpectNull()
    {
        var value = TestData.SomeTextRecordStruct;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.Get<RecordStruct>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableStruct<RecordStruct>();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableStruct_IsNullReturnsFalse_ExpectValueFromProvider()
    {
        var value = TestData.AnotherTextRecordStruct;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.Get<RecordStruct>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableStruct<RecordStruct>();

        Assert.StrictEqual(value, actual);
    }
}
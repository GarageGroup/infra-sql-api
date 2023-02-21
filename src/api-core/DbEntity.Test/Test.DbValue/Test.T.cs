using Moq;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToRefType_IsNullReturnsTrue_ExpectNull()
    {
        var value = TestData.PlusFifteenIdRefType;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.Get<RefType>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastTo<RefType>();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToRefType_IsNullReturnsFalse_ExpectValueFromProvider()
    {
        var value = TestData.MinusFifteenIdRefType;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.Get<RefType>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastTo<RefType>();

        Assert.Equal(value, actual);
    }

    [Fact]
    public static void CastToNullableStructType_IsNullReturnsTrue_ExpectNull()
    {
        var value = TestData.SomeTextRecordStruct;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.Get<RecordStruct>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullable<RecordStruct>();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableStructType_IsNullReturnsFalse_ExpectValueFromProvider()
    {
        var value = TestData.AnotherTextRecordStruct;
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.Get<RecordStruct>() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullable<RecordStruct>();

        Assert.Equal(value, actual);
    }
}
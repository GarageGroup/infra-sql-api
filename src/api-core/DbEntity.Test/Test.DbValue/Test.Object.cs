using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToObject_ExpectValueFromProvider()
    {
        var value = new object();
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.Get() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToObject();

        Assert.Equal(value, actual);
    }

    [Fact]
    public static void CastToNullableObject_IsNullReturnsTrue_ExpectNull()
    {
        var value = new object();
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.Get() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableObject();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableObject_IsNullReturnsFalse_ExpectValueFromProvider()
    {
        var value = new object();
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.Get() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableObject();

        Assert.Equal(value, actual);
    }
}
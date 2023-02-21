using Moq;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Theory]
    [InlineData(null)]
    [InlineData(TestData.SomeString)]
    public static void CastToString_IsNullReturnsTrue_ExpectNull(string? value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetString() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToString();

        Assert.Null(actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(TestData.AnotherString)]
    public static void CastToString_IsNullReturnsFalse_ExpectStringValueFromProvider(string? value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetString() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToString();

        Assert.Equal(value, actual);
    }

    [Fact]
    public static void ImplicitToString_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        string? actual = dbValue;

        Assert.Null(actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(TestData.AnotherString)]
    public static void ImplicitToString_IsNullReturnsTrue_ExpectNull(string? value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetString() == value);

        var dbValue = new DbValue(dbValueProvider);
        string? actual = dbValue;

        Assert.Null(actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(TestData.SomeString)]
    public static void ImplicitToString_IsNullReturnsFalse_ExpectStringValueFromProvider(string? value)
    {
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetString() == value);

        var dbValue = new DbValue(dbValueProvider);
        string? actual = dbValue;

        Assert.Equal(value, actual);
    }
}
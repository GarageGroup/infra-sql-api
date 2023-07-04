using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbValueTest
{
    [Fact]
    public static void CastToGuid_ExpectGuidValueFromProvider()
    {
        var value = Guid.Parse("efb70ac3-ae54-4a13-9f15-440c5a30e5c4");
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetGuid() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToGuid();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void CastToNullableGuid_IsNullReturnsTrue_ExpectNull()
    {
        var value = Guid.Parse("a9e6af1a-2333-40b1-ba5f-cf30e80ab406");
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetGuid() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableGuid();

        Assert.Null(actual);
    }

    [Fact]
    public static void CastToNullableGuid_IsNullReturnsFalse_ExpectGuidValueFromProvider()
    {
        var value = Guid.Parse("2b64b86f-f7db-4015-b299-636681e7dada");
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetGuid() == value);

        var dbValue = new DbValue(dbValueProvider);
        var actual = dbValue.CastToNullableGuid();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToGuid_DbValueIsNull_ExpectArgumentNullException()
    {
        DbValue dbValue = null!;

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("dbValue", ex.ParamName);

        void Test()
            =>
            _ = (Guid)dbValue;
    }

    [Fact]
    public static void ImplicitToGuid_DbValueIsNotNull_ExpectGuidValueFromProvider()
    {
        var value = Guid.Parse("74dfec79-3263-455f-b24a-a69ab231539f");
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.GetGuid() == value);

        var dbValue = new DbValue(dbValueProvider);
        Guid actual = dbValue;

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void ImplicitToNullableGuid_DbValueIsNull_ExpectNull()
    {
        DbValue? dbValue = null;
        Guid? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableGuid_IsNullReturnsTrue_ExpectNull()
    {
        var value = Guid.Parse("250537f9-2416-490d-9584-48e0ae11fdbb");
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == true && db.GetGuid() == value);

        var dbValue = new DbValue(dbValueProvider);
        Guid? actual = dbValue;

        Assert.Null(actual);
    }

    [Fact]
    public static void ImplicitToNullableGuid_IsNullReturnsFalse_ExpectGuidValueFromProvider()
    {
        var value = Guid.Parse("cbf7e890-f472-4c2c-ac41-9120241d360c");
        var dbValueProvider = Mock.Of<IDbValueProvider>(db => db.IsNull() == false && db.GetGuid() == value);

        var dbValue = new DbValue(dbValueProvider);
        Guid? actual = dbValue;

        Assert.StrictEqual(value, actual);
    }
}
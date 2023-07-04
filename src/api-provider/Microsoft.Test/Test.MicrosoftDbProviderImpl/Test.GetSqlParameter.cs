using System;
using Microsoft.Data.SqlClient;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Microsoft.Test;

partial class MicrosoftDbProviderImplTest
{
    [Fact]
    public static void GetSqlParameter_SourceParameterIsNull_ExpectArgumentNullException()
    {
        var option = new MicrosoftDbProviderOption(SomeConnectionString, SomeRetryLogicOption);
        var dbProvider = MicrosoftDbProviderImpl.InternalCreate(option);

        var ex = Assert.Throws<ArgumentNullException>(Test);
        Assert.Equal("parameter", ex.ParamName);

        void Test()
            =>
            _ = dbProvider.GetSqlParameter(null!);
    }

    [Fact]
    public static void GetSqlParameter_SourceParameterValueIsNull_ExpectSqlParameterValueIsDbNull()
    {
        var option = new MicrosoftDbProviderOption(SomeConnectionString, SomeRetryLogicOption);
        var dbProvider = MicrosoftDbProviderImpl.InternalCreate(option);

        var parameter = new DbParameter("SomeName", null);
        var actual = dbProvider.GetSqlParameter(parameter);

        var actualSqlParameter = Assert.IsType<SqlParameter>(actual);

        Assert.Equal("SomeName", actualSqlParameter.ParameterName);
        Assert.Equal(DBNull.Value, actualSqlParameter.Value);

        Assert.True(actualSqlParameter.IsNullable);
    }

    [Theory]
    [InlineData(TestData.EmptyString)]
    [InlineData(TestData.WhiteSpaceString)]
    [InlineData(TestData.SomeString)]
    [InlineData(TestData.Zero)]
    [InlineData(TestData.PlusFifteen)]
    public static void GetSqlParameter_SourceParameterValueIsNotNull_ExpectSqlParameterValueIsEqualToSourceValue(
        object sourceValue)
    {
        var option = new MicrosoftDbProviderOption(SomeConnectionString, SomeRetryLogicOption);
        var dbProvider = MicrosoftDbProviderImpl.InternalCreate(option);

        var parameter = new DbParameter("Some name", sourceValue);
        var actual = dbProvider.GetSqlParameter(parameter);

        var actualSqlParameter = Assert.IsType<SqlParameter>(actual);

        Assert.Equal("Some name", actualSqlParameter.ParameterName);
        Assert.Equal(sourceValue, actualSqlParameter.Value);

        Assert.False(actualSqlParameter.IsNullable);
    }
}
using Microsoft.Data.SqlClient;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Microsoft.Test;

partial class MicrosoftDbProviderImplTest
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void GetDbConnection_ExpectConnectionStringIsEqualToSourceConnectionString(bool isRetryLogicOptionPresent)
    {
        var option = new MicrosoftDbProviderOption(
            connectionString: SomeConnectionString,
            retryOption: isRetryLogicOptionPresent ? SomeRetryLogicOption : null);

        var dbProvider = MicrosoftDbProviderImpl.InternalCreate(option);
        using var actualDbConnection = dbProvider.GetDbConnection();

        var actualSqlConnection = Assert.IsType<SqlConnection>(actualDbConnection);
        Assert.Equal(SomeConnectionString, actualSqlConnection.ConnectionString);
    }
}
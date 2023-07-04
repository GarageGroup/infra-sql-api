using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace GarageGroup.Infra;

partial class MicrosoftDbProviderImpl
{
    public DbConnection GetDbConnection()
        =>
        new SqlConnection(connectionString)
        {
            RetryLogicProvider = retryLogicProvider
        };
}
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace GGroupp.Infra;

partial class MicrosoftDbProviderImpl
{
    public DbConnection GetDbConnection()
        =>
        new SqlConnection(connectionString)
        {
            RetryLogicProvider = retryLogicBaseProvider
        };
}
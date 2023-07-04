using Microsoft.Data.SqlClient;

namespace GarageGroup.Infra;

public sealed record class MicrosoftDbProviderOption
{
    public MicrosoftDbProviderOption(string connectionString, SqlRetryLogicOption? retryOption)
    {
        ConnectionString = connectionString ?? string.Empty;
        RetryOption = retryOption;
    }

    public string ConnectionString { get; }

    public SqlRetryLogicOption? RetryOption { get; }
}
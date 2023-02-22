using System.Collections.Concurrent;
using Microsoft.Data.SqlClient;

namespace GGroupp.Infra;

internal sealed partial class MicrosoftDbProviderImpl : IDbProvider
{
    private static readonly ConcurrentDictionary<string, SqlRetryLogicBaseProvider> RetryProviders;

    static MicrosoftDbProviderImpl()
        =>
        RetryProviders = new();

    internal static MicrosoftDbProviderImpl InternalCreate(MicrosoftDbProviderOption option)
    {
        if (option.RetryOption is null)
        {
            return new(option.ConnectionString, null);
        }

        var retryLogicProvider = RetryProviders.GetOrAdd(option.ConnectionString, CreateRetryLogicProvider);
        return new(option.ConnectionString, retryLogicProvider);

        SqlRetryLogicBaseProvider CreateRetryLogicProvider(string connectionString)
            =>
            SqlConfigurableRetryFactory.CreateExponentialRetryProvider(option.RetryOption);
    }

    private readonly string connectionString;

    private readonly SqlRetryLogicBaseProvider? retryLogicProvider;

    private MicrosoftDbProviderImpl(string connectionString, SqlRetryLogicBaseProvider? retryLogicProvider)
    {
        this.connectionString = connectionString;
        this.retryLogicProvider = retryLogicProvider;
    }
}
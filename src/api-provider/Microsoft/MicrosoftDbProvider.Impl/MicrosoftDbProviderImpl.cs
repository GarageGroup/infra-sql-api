using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace GGroupp.Infra;

internal sealed partial class MicrosoftDbProviderImpl : IDbProvider
{
    static MicrosoftDbProviderImpl()
        =>
        RetryProviders = new();

    internal static MicrosoftDbProviderImpl InternalCreate(MicrosoftDbProviderOption option)
    {
        if (option.RetryOption is null)
        {
            return new(option.ConnectionString, null);
        }

        var retryLogicBaseProvider = GetRetryLogicBaseProvider(option.ConnectionString, option.RetryOption);
        return new(option.ConnectionString, retryLogicBaseProvider);
    }

    private static SqlRetryLogicBaseProvider GetRetryLogicBaseProvider(
        string connectionString, SqlRetryLogicOption retryOption)
    {
        if (RetryProviders.TryGetValue(connectionString, out var existedProvider))
        {
            return existedProvider;
        }

        var newProvider = SqlConfigurableRetryFactory.CreateExponentialRetryProvider(retryOption);
        RetryProviders[connectionString] = newProvider;

        return newProvider;
    }

    private static readonly Dictionary<string, SqlRetryLogicBaseProvider> RetryProviders;

    private readonly string connectionString;

    private readonly SqlRetryLogicBaseProvider? retryLogicBaseProvider;

    private MicrosoftDbProviderImpl(string connectionString, SqlRetryLogicBaseProvider? retryLogicBaseProvider)
    {
        this.connectionString = connectionString;
        this.retryLogicBaseProvider = retryLogicBaseProvider;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PrimeFuncPack;

[assembly: InternalsVisibleTo("GGroupp.Infra.Sql.Api.Provider.Microsoft.Test")]

namespace GGroupp.Infra;

public static class MicrosoftDbProvider
{
    private const string RetryPolicyDefaultSectionName = "DbRetryPolicy";

    public static Dependency<IDbProvider> Configure(Func<IServiceProvider, MicrosoftDbProviderOption> optionResolver)
    {
        ArgumentNullException.ThrowIfNull(optionResolver);
        return Dependency.From(optionResolver).Map(CreateMicrosoftDbProvider);
    }

    public static Dependency<IDbProvider> Configure(
        string connectionStringName, string retryPolicySectionName = RetryPolicyDefaultSectionName)
    {
        return Dependency.From(ResolveConfiguration).Map(GetOption).Map<IDbProvider>(MicrosoftDbProviderImpl.InternalCreate);

        static IConfiguration ResolveConfiguration(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>();

        MicrosoftDbProviderOption GetOption(IConfiguration configuration)
            =>
            configuration.GetMicrosoftDbProviderOption(connectionStringName, retryPolicySectionName);
    }

    private static IDbProvider CreateMicrosoftDbProvider(MicrosoftDbProviderOption option)
    {
        ArgumentNullException.ThrowIfNull(option);
        return MicrosoftDbProviderImpl.InternalCreate(option);
    }

    private static MicrosoftDbProviderOption GetMicrosoftDbProviderOption(
        this IConfiguration configuration, string connectionStringName, string? retryPolicySectionName)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName ?? string.Empty);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionStringName}' must be specified");
        }

        var retrySectionName = string.IsNullOrEmpty(retryPolicySectionName) ? RetryPolicyDefaultSectionName : retryPolicySectionName;
        var retrySection = configuration.GetSection(retrySectionName);

        if (retrySection.Exists() is false)
        {
            return new(connectionString, null);
        }

        return new(
            connectionString: connectionString,
            retryOption: new()
            {
                NumberOfTries = retrySection.GetInt32(nameof(SqlRetryLogicOption.NumberOfTries)),
                DeltaTime = retrySection.GetTimeSpan(nameof(SqlRetryLogicOption.DeltaTime)),
                MinTimeInterval = retrySection.GetTimeSpan(nameof(SqlRetryLogicOption.MinTimeInterval)),
                MaxTimeInterval = retrySection.GetTimeSpan(nameof(SqlRetryLogicOption.MaxTimeInterval)),
                TransientErrors = retrySection.GetInt32Collecton(nameof(SqlRetryLogicOption.TransientErrors))
            });
    }

    private static int GetInt32(this IConfigurationSection section, string key)
    {
        var value = section[key];

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        return int.Parse(value);
    }

    private static TimeSpan GetTimeSpan(this IConfigurationSection section, string key)
    {
        var value = section[key];

        if (string.IsNullOrEmpty(value))
        {
            return default;
        }

        return TimeSpan.Parse(value);
    }

    private static IEnumerable<int> GetInt32Collecton(this IConfigurationSection section, string key)
    {
        return section.GetSection(key).AsEnumerable().Select(GetValue).Where(IsNotEmpty).Select(int.Parse).ToArray();

        static string GetValue(KeyValuePair<string, string?> kv)
            =>
            kv.Value ?? string.Empty;

        static bool IsNotEmpty(string item)
            =>
            string.IsNullOrEmpty(item) is false;
    }
}
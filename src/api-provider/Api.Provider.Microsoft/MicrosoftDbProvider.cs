using Microsoft.Extensions.Configuration;
using PrimeFuncPack;
using System;

namespace GGroupp.Infra;

public static class MicrosoftDbProvider
{
    public static Dependency<IDbProvider> Configure(Func<IServiceProvider, MicrosoftDbProviderOption> optionResolver)
    {
        _ = optionResolver ?? throw new ArgumentNullException(nameof(optionResolver));

        return Dependency.From(optionResolver).Map(GetConnectionString).InnerUseMicrosoftDbProvider();

        static string GetConnectionString(MicrosoftDbProviderOption option)
            =>
            option?.ConnectionString ?? string.Empty;
    }

    public static Dependency<IDbProvider> Configure(string connectionStringName)
    {
        return Dependency.From(ResolveConfiguration).Map(GetConnectionString).InnerUseMicrosoftDbProvider();

        static IConfiguration ResolveConfiguration(IServiceProvider serviceProvider)
            =>
            serviceProvider.GetServiceOrThrow<IConfiguration>();

        string GetConnectionString(IConfiguration configuration)
            =>
            configuration.GetConnectionStringOrThrow(connectionStringName ?? string.Empty);
    }

    private static Dependency<IDbProvider> InnerUseMicrosoftDbProvider(this Dependency<string> dependency)
        =>
        dependency.Map<IDbProvider>(MicrosoftDbProviderImpl.Create);

    private static string GetConnectionStringOrThrow(this IConfiguration configuration, string connectionStringName)
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{connectionStringName}' must be specified");
        }

        return connectionString;
    }
}
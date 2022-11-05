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
            configuration.GetConnectionString(connectionStringName);
    }

    private static Dependency<IDbProvider> InnerUseMicrosoftDbProvider(this Dependency<string> dependency)
        =>
        dependency.Map<IDbProvider>(MicrosoftDbProviderImpl.Create);
}
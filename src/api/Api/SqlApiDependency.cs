using Microsoft.Extensions.Logging;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Infra.Sql.Api.Provider.Api.Test")]

namespace GGroupp.Infra;

public static class SqlApiDependency
{
    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.With(GetLoggerFactory).Fold<ISqlApi>(CreateSqlApi);
    }

    private static ILoggerFactory? GetLoggerFactory(this IServiceProvider serviceProvider)
        =>
        serviceProvider.GetServiceOrAbsent<ILoggerFactory>().OrDefault();

    private static SqlApi CreateSqlApi(IDbProvider provider, ILoggerFactory? loggerFactory)
        =>
        new(provider, loggerFactory);
}
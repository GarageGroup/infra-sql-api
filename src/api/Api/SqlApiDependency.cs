using Microsoft.Extensions.Logging;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Infra.Sql.Api.Provider.Api.Test")]

namespace GGroupp.Infra;

public static class SqlApiDependency
{
    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider> dependency, bool useLogging = false)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.With(serviceProvider => GetLoggerFactory(serviceProvider, useLogging)).Fold<ISqlApi>(CreateSqlApi);
    }

    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider, ILoggerFactory> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ISqlApi>(CreateSqlApi);
    }

    private static ILoggerFactory? GetLoggerFactory(this IServiceProvider serviceProvider, bool useLogging)
        =>
        useLogging ? serviceProvider.GetServiceOrAbsent<ILoggerFactory>().OrDefault() : null;

    private static SqlApi CreateSqlApi(IDbProvider provider, ILoggerFactory? loggerFactory)
        =>
        new(provider ?? throw new ArgumentNullException(nameof(provider)), loggerFactory);
}
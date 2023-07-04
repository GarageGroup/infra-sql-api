using Microsoft.Extensions.Logging;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Infra.Sql.Api.Provider.Api.Test")]

namespace GarageGroup.Infra;

public static class SqlApiDependency
{
    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider> dependency, bool useLogging = false)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.With(GetLoggerFactory).Fold<ISqlApi>(CreateSqlApi);

        ILoggerFactory? GetLoggerFactory(IServiceProvider serviceProvider)
            =>
            useLogging ? serviceProvider.GetServiceOrAbsent<ILoggerFactory>().OrDefault() : null;
    }

    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider, ILoggerFactory> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ISqlApi>(CreateSqlApi);
    }

    private static SqlApi CreateSqlApi(IDbProvider provider, ILoggerFactory? loggerFactory)
        =>
        new(provider ?? throw new ArgumentNullException(nameof(provider)), loggerFactory);
}
using Microsoft.Extensions.Logging;
using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Infra.Sql.Api.Provider.Api.Test")]

namespace GarageGroup.Infra;

public static class SqlApiDependency
{
    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ISqlApi>(InnerCreateSqlApi);

        static SqlApi InnerCreateSqlApi(IServiceProvider serviceProvider, IDbProvider dbProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(dbProvider);

            return new(
                dbProvider: dbProvider,
                loggerFactory: serviceProvider.GetServiceOrThrow<ILoggerFactory>());
        }
    }

    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider> dependency, bool useLogging)
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

    private static SqlApi CreateSqlApi(IDbProvider dbProvider, ILoggerFactory? loggerFactory)
        =>
        new(dbProvider ?? throw new ArgumentNullException(nameof(dbProvider)), loggerFactory);
}
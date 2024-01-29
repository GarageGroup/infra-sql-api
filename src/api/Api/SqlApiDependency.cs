using Microsoft.Extensions.Logging;
using PrimeFuncPack;
using System;
using System.Data.Common;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GarageGroup.Infra.Sql.Api.Provider.Api.Test")]

namespace GarageGroup.Infra;

public static class SqlApiDependency
{
    public static Dependency<ISqlApi> UseSqlApi<TDbConnection>(this Dependency<IDbProvider<TDbConnection>> dependency)
        where TDbConnection : DbConnection
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ISqlApi>(InnerCreateSqlApi);

        static SqlApi<TDbConnection> InnerCreateSqlApi(IServiceProvider serviceProvider, IDbProvider<TDbConnection> dbProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(dbProvider);

            return new(
                dbProvider: dbProvider,
                loggerFactory: serviceProvider.GetServiceOrThrow<ILoggerFactory>());
        }
    }

    public static Dependency<ISqlApi> UseSqlApi<TDbConnection>(this Dependency<IDbProvider<TDbConnection>> dependency, bool useLogging)
        where TDbConnection : DbConnection
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.With(GetLoggerFactory).Fold<ISqlApi>(CreateSqlApi);

        ILoggerFactory? GetLoggerFactory(IServiceProvider serviceProvider)
            =>
            useLogging ? serviceProvider.GetServiceOrAbsent<ILoggerFactory>().OrDefault() : null;
    }

    public static Dependency<ISqlApi> UseSqlApi<TDbConnection>(this Dependency<IDbProvider<TDbConnection>, ILoggerFactory> dependency)
        where TDbConnection : DbConnection
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ISqlApi>(CreateSqlApi);
    }

    private static SqlApi<TDbConnection> CreateSqlApi<TDbConnection>(IDbProvider<TDbConnection> dbProvider, ILoggerFactory? loggerFactory)
        where TDbConnection : DbConnection
    {
        ArgumentNullException.ThrowIfNull(dbProvider);
        return new(dbProvider, loggerFactory);
    }
}
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
        return dependency.Map<ISqlApi>(InnerCreateSqlApi);

        SqlApi<TDbConnection> InnerCreateSqlApi(IServiceProvider serviceProvider, IDbProvider<TDbConnection> dbProvider)
        {
            ArgumentNullException.ThrowIfNull(serviceProvider);
            ArgumentNullException.ThrowIfNull(dbProvider);

            return new(
                dbProvider: dbProvider,
                loggerFactory: useLogging ? serviceProvider.GetServiceOrThrow<ILoggerFactory>() : null);
        }
    }

    public static Dependency<ISqlApi> UseSqlApi<TDbConnection>(this Dependency<IDbProvider<TDbConnection>, ILoggerFactory> dependency)
        where TDbConnection : DbConnection
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Fold<ISqlApi>(InnerCreateSqlApi);

        static SqlApi<TDbConnection> InnerCreateSqlApi(IDbProvider<TDbConnection> dbProvider, ILoggerFactory? loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(dbProvider);
            return new(dbProvider, loggerFactory);
        }
    }
}
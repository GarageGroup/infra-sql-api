using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class SqlApi : ISqlApi
{
    public static SqlApi Create(IDbProvider dbProvider, ILoggerFactory? loggerFactory)
        =>
        new(
            dbProvider: dbProvider ?? throw new ArgumentNullException(nameof(dbProvider)),
            logger: loggerFactory?.CreateLogger<SqlApi>());

    private readonly IDbProvider dbProvider;

    private readonly ILogger? logger;

    private SqlApi(IDbProvider dbProvider, ILogger? logger)
        =>
        (this.dbProvider, this.logger) = (dbProvider, logger);

    private DbCommand CreateDbCommand(DbConnection dbConnection, IDbQuery query)
    {
        var dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query.GetSqlQuery();
        var parameterLogBuilder = logger is null ? null : new StringBuilder();

        foreach (var sqlParameter in GetDistinctDbParameters(query))
        {
            AppendParameter(sqlParameter);
            dbCommand.Parameters.Add(dbProvider.GetSqlParameter(sqlParameter));
        }

        if (query.TimeoutInSeconds is not null)
        {
            dbCommand.CommandTimeout = query.TimeoutInSeconds.Value;
        }

        logger?.LogDebug("SQL: {0}, Parameters: {1}", dbCommand.CommandText, parameterLogBuilder?.ToString());  

        return dbCommand;

        void AppendParameter(DbParameter dbParameter)
        {
            if (parameterLogBuilder is null)
            {
                return;
            }

            if (parameterLogBuilder.Length > 0)
            {
                parameterLogBuilder.Append(", ");
            }

            parameterLogBuilder
                .Append(dbParameter.Name)
                .Append(": ")
                .Append(dbParameter.Value)
                .Append("");
        }
    }

    private static IEnumerable<DbParameter> GetDistinctDbParameters(IDbQuery query)
    {
        var dbParameters = query.GetParameters();
        var dbNameParameters = new Dictionary<string, DbParameter>(dbParameters.Length);

        foreach (var dbParameter in dbParameters)
        {
            dbNameParameters[dbParameter.Name] = dbParameter;
        }

        return dbNameParameters.Values;
    }

    private static IReadOnlyDictionary<string, int> CreateFieldIndexes(DbDataReader dbDataReader)
        =>
        Enumerable.Range(0, dbDataReader.FieldCount).ToDictionary(dbDataReader.GetName, static index => index);
}

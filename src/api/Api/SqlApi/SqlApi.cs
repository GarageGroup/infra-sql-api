using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GGroupp.Infra;

internal sealed partial class SqlApi : ISqlApi
{
    public static SqlApi Create(IDbProvider dbProvider, bool needLogging, ILoggerFactory? loggerFactory)
        =>
        new(
            dbProvider: dbProvider ?? throw new ArgumentNullException(nameof(dbProvider)),
            logger: needLogging ? loggerFactory?.CreateLogger<SqlApi>() : default);

    private readonly IDbProvider dbProvider;

    private readonly ILogger? logger;

    private SqlApi(IDbProvider dbProvider, ILogger? logger)
        =>
        (this.dbProvider, this.logger) = (dbProvider, logger);

    private DbCommand CreateDbCommand(DbConnection dbConnection, IDbQuery query)
    {
        var dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query.GetSqlQuery();
        var parametersLogBuilder = new StringBuilder();

        foreach (var sqlParameter in GetDistinctDbParameters(query))
        {
            AppendParameter(parametersLogBuilder, sqlParameter);
            dbCommand.Parameters.Add(dbProvider.GetSqlParameter(sqlParameter));
        }

        if (query.TimeoutInSeconds is not null)
        {
            dbCommand.CommandTimeout = query.TimeoutInSeconds.Value;
        }

        logger?.LogDebug("SQL: {0}, Parameters: {1}", dbCommand.CommandText, parametersLogBuilder.ToString());  

        return dbCommand;

        void AppendParameter(StringBuilder builder, DbParameter dbParameter)
        {
            if (logger is null)
            {
                return;
            }

            if (builder.Length > 0)
            {
                builder.Append(", ");
            }

            builder
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

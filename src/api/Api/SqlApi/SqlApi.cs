using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra;

internal sealed partial class SqlApi : ISqlApi
{
    private const string PingQuery = "SELECT 1;";

    private readonly IDbProvider dbProvider;

    private readonly ILogger? logger;

    internal SqlApi(IDbProvider dbProvider, ILoggerFactory? loggerFactory = null)
    {
        this.dbProvider = dbProvider;
        logger = loggerFactory?.CreateLogger<SqlApi>();
    }

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

        logger?.LogDebug("SQL: {sql}, Parameters: {parameters}", dbCommand.CommandText, parameterLogBuilder?.ToString());

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
    {
        var fieldCount = dbDataReader.FieldCount;
        var filedIndexes = new Dictionary<string, int>(capacity: fieldCount);

        for (var index = 0; index < fieldCount; index++)
        {
            var fieldName = dbDataReader.GetName(index);
            if (filedIndexes.ContainsKey(fieldName))
            {
                continue;
            }

            filedIndexes.Add(fieldName, index);
        }

        return filedIndexes;
    }
}

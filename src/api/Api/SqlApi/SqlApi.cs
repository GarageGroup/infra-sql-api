using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.Extensions.Logging;

namespace GarageGroup.Infra;

internal sealed partial class SqlApi<TDbConnection> : ISqlApi
    where TDbConnection : DbConnection
{
    private const string PingQuery = "SELECT 1;";

    private readonly IDbProvider<TDbConnection> dbProvider;

    private readonly ILogger? logger;

    internal SqlApi(IDbProvider<TDbConnection> dbProvider, ILoggerFactory? loggerFactory = null)
    {
        this.dbProvider = dbProvider;
        logger = loggerFactory?.CreateLogger("GarageGroup.Infra.SqlApi");
    }

    private DbCommand CreateDbCommand(TDbConnection dbConnection, IDbQuery query)
    {
        var dbParameters = query.GetParameters();
        var dbNameParameters = new Dictionary<string, DbParameter>(dbParameters.Length);

        var parameterLogBuilder = logger is null ? null : new StringBuilder();

        foreach (var dbParameter in dbParameters)
        {
            if (dbNameParameters.ContainsKey(dbParameter.Name))
            {
                continue;
            }

            dbNameParameters[dbParameter.Name] = dbParameter;

            if (parameterLogBuilder is null)
            {
                continue;
            }

            if (parameterLogBuilder.Length > 0)
            {
                parameterLogBuilder.Append(", ");
            }

            parameterLogBuilder.Append(dbParameter.Name).Append(": '").Append(dbParameter.Value).Append('\'');
        }

        var commandText = query.GetSqlQuery(dbProvider.Dialect);

        logger?.LogDebug("SQL: {sql}. Parameters: {parameters}", commandText, parameterLogBuilder?.ToString());
        return dbProvider.GetDbCommand(dbConnection, commandText, dbNameParameters.Values, query.TimeoutInSeconds);
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

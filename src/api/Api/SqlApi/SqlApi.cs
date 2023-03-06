using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace GGroupp.Infra;

internal sealed partial class SqlApi : ISqlApi
{
    public static SqlApi Create(IDbProvider dbProvider)
        =>
        new(
            dbProvider ?? throw new ArgumentNullException(nameof(dbProvider)));

    private readonly IDbProvider dbProvider;

    private SqlApi(IDbProvider dbProvider)
        =>
        this.dbProvider = dbProvider;

    private DbCommand CreateDbCommand(DbConnection dbConnection, IDbQuery query)
    {
        var dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = query.GetSqlQuery();

        foreach (var sqlParameter in GetDistinctDbParameters(query).Select(dbProvider.GetSqlParameter))
        {
            dbCommand.Parameters.Add(sqlParameter);
        }

        if (query.TimeoutInSeconds is not null)
        {
            dbCommand.CommandTimeout = query.TimeoutInSeconds.Value;
        }

        return dbCommand;
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

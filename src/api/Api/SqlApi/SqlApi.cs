using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace GGroupp.Infra;

internal sealed partial class SqlApi : ISqlApi
{
    internal static SqlApi Create(IDbProvider dbProvider)
        =>
        new(
            dbProvider ?? throw new ArgumentNullException(nameof(dbProvider)));

    private readonly IDbProvider dbProvider;

    private SqlApi(IDbProvider dbProvider)
        =>
        this.dbProvider = dbProvider;

    private DbCommand CreateDbCommand(DbConnection dbConnection, SqlRequest request)
    {
        var dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = request.Query;

        foreach (var sqlParameter in request.Parameters.Select(dbProvider.GetDbParameter))
        {
            dbCommand.Parameters.Add(sqlParameter);
        }

        return dbCommand;
    }

    private static IReadOnlyDictionary<string, int> CreateFieldIndexes(DbDataReader dbDataReader)
        =>
        Enumerable.Range(0, dbDataReader.FieldCount).ToDictionary(dbDataReader.GetName, static index => index);
}

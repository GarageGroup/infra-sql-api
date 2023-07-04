using System;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

internal sealed partial class StubDbQuery : IDbQuery
{
    private readonly string sqlQuery;

    private readonly FlatArray<DbParameter> parameters;

    public StubDbQuery(string sqlQuery, params DbParameter[] parameters)
    {
        this.sqlQuery = sqlQuery;
        this.parameters = parameters;
    }

    public int? TimeoutInSeconds { get; init; }

    public string GetSqlQuery()
        =>
        sqlQuery;

    public FlatArray<DbParameter> GetParameters()
        =>
        parameters;
}
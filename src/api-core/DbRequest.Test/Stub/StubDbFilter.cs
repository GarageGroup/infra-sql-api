using System;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

internal sealed partial class StubDbFilter : IDbFilter
{
    private readonly string sqlQuery;

    private readonly FlatArray<DbParameter> parameters;

    public StubDbFilter(string sqlQuery, params DbParameter[] parameters)
    {
        this.sqlQuery = sqlQuery;
        this.parameters = parameters;
    }

    public string GetFilterSqlQuery()
        =>
        sqlQuery;

    public FlatArray<DbParameter> GetFilterParameters()
        =>
        parameters;
}
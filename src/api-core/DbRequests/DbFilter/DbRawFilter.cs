using System;

namespace GGroupp.Infra;

public sealed record class DbRawFilter : IDbFilter
{
    public DbRawFilter(string sqlQuery)
        =>
        SqlQuery = sqlQuery.OrEmpty();

    public string SqlQuery { get; }

    public FlatArray<DbParameter> Parameters { get; init; }

    public string GetFilterSqlQuery()
        =>
        SqlQuery;

    public FlatArray<DbParameter> GetFilterParameters()
        =>
        Parameters;
}
using System;

namespace GGroupp.Infra;

public sealed record class DbDeleteQuery : IDbQuery
{
    public DbDeleteQuery(string tableName, IDbFilter filter)
    {
        TableName = tableName.OrEmpty();
        Filter = filter;
    }

    public string TableName { get; }

    public IDbFilter Filter { get; }

    public string GetSqlQuery()
        =>
        this.BuildSqlQuery();

    public FlatArray<DbParameter> GetParameters()
        =>
        this.BuildParameters();
}
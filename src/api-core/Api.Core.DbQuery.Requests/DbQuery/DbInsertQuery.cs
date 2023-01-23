using System;

namespace GGroupp.Infra;

public sealed record class DbInsertQuery : IDbQuery
{
    public DbInsertQuery(string tableName, FlatArray<DbFieldValue> fieldValues)
    {
        TableName = tableName.OrEmpty();
        FieldValues = fieldValues;
    }

    public string TableName { get; }

    public FlatArray<DbFieldValue> FieldValues { get; }

    public string GetSqlQuery()
        =>
        this.BuildSqlQuery();

    public FlatArray<DbParameter> GetParameters()
        =>
        this.BuildParameters();
}
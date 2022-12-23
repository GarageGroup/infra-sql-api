using System;

namespace GGroupp.Infra;

public sealed record class DbRequest : IDbQuery
{
    public DbRequest(string tableName)
        =>
        TableName = tableName ?? string.Empty;

    public DbRequest(string tableName, string shortName)
    {
        TableName = tableName ?? string.Empty;
        ShortName = string.IsNullOrWhiteSpace(shortName) ? null : shortName;
    }

    public string TableName { get; }

    public string? ShortName { get; }

    public int? Top { get; init; }

    public long? Offset { get; init; }

    public FlatArray<string> SelectedFields { get; init; }

    public FlatArray<IDbFilter> Filters { get; init; }

    public FlatArray<DbJoinedTable> JoinedTables { get; init; }

    public FlatArray<DbOrder> Orders { get; init; }

    string IDbQuery.GetSqlQuery()
        =>
        this.BuildSqlQuery();

    FlatArray<DbParameter> IDbQuery.GetParameters()
        =>
        this.BuildParameters();
}
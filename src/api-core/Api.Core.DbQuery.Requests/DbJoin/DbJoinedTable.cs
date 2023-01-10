namespace GGroupp.Infra;

public sealed record class DbJoinedTable
{
    public DbJoinedTable(DbJoinType type, string tableName, string shortName, IDbFilter filter)
    {
        Type = type;
        TableName = tableName ?? string.Empty;
        ShortName = shortName ?? string.Empty;
        Filter = filter;
    }

    public DbJoinType Type { get; }

    public string TableName { get; }

    public string ShortName { get; }

    public IDbFilter Filter { get; }
}
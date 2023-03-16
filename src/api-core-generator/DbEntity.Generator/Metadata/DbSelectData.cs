namespace GGroupp.Infra;

internal sealed record class DbSelectData
{
    public DbSelectData(string queryName, DbJoinData? joinTable, string fieldName)
    {
        QueryName = queryName ?? string.Empty;
        JoinTable = joinTable;
        FieldName = fieldName ?? string.Empty;
    }

    public string QueryName { get; }

    public DbJoinData? JoinTable { get; }

    public string FieldName { get; }
}
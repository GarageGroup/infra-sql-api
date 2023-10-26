namespace GarageGroup.Infra;

internal sealed record class DbSelectData
{
    public DbSelectData(
        string queryName,
        DbJoinData? joinTable,
        string fieldName,
        string? aliasName,
        bool groupBy)
    {
        QueryName = queryName ?? string.Empty;
        JoinTable = joinTable;
        FieldName = fieldName ?? string.Empty;
        AliasName = aliasName;
        GroupBy = groupBy;
    }

    public string QueryName { get; }

    public DbJoinData? JoinTable { get; }

    public string FieldName { get; }

    public string? AliasName { get; }

    public bool GroupBy { get; }
}
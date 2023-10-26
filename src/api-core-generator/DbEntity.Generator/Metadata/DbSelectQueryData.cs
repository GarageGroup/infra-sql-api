using System;
using System.Collections.Generic;

namespace GarageGroup.Infra;

internal sealed record class DbSelectQueryData
{
    public DbSelectQueryData(
        string queryName,
        DbTableData tableData,
        IReadOnlyList<DbJoinData>? joinedTables,
        IReadOnlyList<string>? fieldNames,
        IReadOnlyList<string>? groupByFields)
    {
        QueryName = queryName ?? string.Empty;
        TableData = tableData;
        JoinedTables = joinedTables ?? Array.Empty<DbJoinData>();
        FieldNames = fieldNames ?? Array.Empty<string>();
        GroupByFields = groupByFields ?? Array.Empty<string>();
    }

    public string QueryName { get; }

    public DbTableData TableData { get; }

    public IReadOnlyList<DbJoinData> JoinedTables { get; }

    public IReadOnlyList<string> FieldNames { get; }

    public IReadOnlyList<string> GroupByFields { get; }
}
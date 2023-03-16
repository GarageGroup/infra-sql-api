using System;
using System.Collections.Generic;

namespace GGroupp.Infra;

internal sealed record class DbSelectQueryData
{
    public DbSelectQueryData(
        string queryName,
        DbTableData tableData,
        IReadOnlyList<DbJoinData>? joinedTables,
        IReadOnlyList<string>? fieldNames)
    {
        QueryName = queryName ?? string.Empty;
        TableData = tableData;
        JoinedTables = joinedTables ?? Array.Empty<DbJoinData>();
        FieldNames = fieldNames ?? Array.Empty<string>();
    }

    public string QueryName { get; }

    public DbTableData TableData { get; }

    public IReadOnlyList<DbJoinData> JoinedTables { get; }

    public IReadOnlyList<string> FieldNames { get; }
}
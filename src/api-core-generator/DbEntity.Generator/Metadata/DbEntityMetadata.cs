using System;
using System.Collections.Generic;

namespace GGroupp.Infra;

internal sealed record class DbEntityMetadata
{
    public DbEntityMetadata(
        string fileName,
        DisplayedTypeData entityType,
        bool isRecordType,
        bool isValueType,
        IReadOnlyList<DbFieldMetadata> fields)
    {
        FileName = fileName;
        EntityType = entityType;
        IsRecordType = isRecordType;
        IsValueType = isValueType;
        Fields = fields ?? Array.Empty<DbFieldMetadata>();
    }

    public string FileName { get; }

    public DisplayedTypeData EntityType { get; }

    public bool IsRecordType { get; }

    public bool IsValueType { get; }

    public IReadOnlyList<DbFieldMetadata> Fields { get; }
}
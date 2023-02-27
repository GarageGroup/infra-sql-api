using System;
using System.Collections.Generic;

namespace GGroupp.Infra;

internal sealed record class DbEntityMetadata
{
    public DbEntityMetadata(string fileName, DisplayedTypeData entityType, IReadOnlyList<DbFieldMetadata> fields)
    {
        FileName = fileName;
        EntityType = entityType;
        Fields = fields ?? Array.Empty<DbFieldMetadata>();
    }

    public string FileName { get; }

    public DisplayedTypeData EntityType { get; }

    public IReadOnlyList<DbFieldMetadata> Fields { get; }
}
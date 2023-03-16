using System;
using System.Collections.Generic;

namespace GGroupp.Infra;

internal sealed record class DbEntityMetadata
{
    public DbEntityMetadata(
        string fileName,
        DbEntityType entityType,
        IReadOnlyList<DbFieldMetadata> fields,
        IReadOnlyList<DbSelectQueryData> selectQueries)
    {
        FileName = fileName;
        EntityType = entityType;
        Fields = fields ?? Array.Empty<DbFieldMetadata>();
        SelectQueries = selectQueries ?? Array.Empty<DbSelectQueryData>();
    }

    public string FileName { get; }

    public DbEntityType EntityType { get; }

    public IReadOnlyList<DbFieldMetadata> Fields { get; }

    public IReadOnlyList<DbSelectQueryData> SelectQueries { get; }
}
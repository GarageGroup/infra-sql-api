using System;
using System.Collections.Generic;

namespace GarageGroup.Infra;

internal sealed record class DbEntityMetadata
{
    public DbEntityMetadata(
        string fileName,
        DbEntityType entityType,
        IReadOnlyList<DbFieldMetadata> fields,
        IReadOnlyList<DbSelectQueryData> selectQueries,
        DbExtensionFieldMetadata? extensionField)
    {
        FileName = fileName;
        EntityType = entityType;
        Fields = fields ?? Array.Empty<DbFieldMetadata>();
        SelectQueries = selectQueries ?? Array.Empty<DbSelectQueryData>();
        ExtensionField = extensionField;
    }

    public string FileName { get; }

    public DbEntityType EntityType { get; }

    public IReadOnlyList<DbFieldMetadata> Fields { get; }

    public IReadOnlyList<DbSelectQueryData> SelectQueries { get; }

    public DbExtensionFieldMetadata? ExtensionField { get; }
}
using System;

namespace GarageGroup.Infra;

partial class DbItem
{
    public DbValue GetFieldValueOrThrow(string fieldName)
    {
        var name = fieldName ?? string.Empty;
        var fieldIndex = GetFieldIndex(name);

        if (fieldIndex is null)
        {
            throw new InvalidOperationException($"Field '{name}' must be present in the data reader");
        }

        return GetDbValue(fieldIndex.Value, name);
    }
}
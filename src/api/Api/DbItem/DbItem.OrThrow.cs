using System;

namespace GGroupp.Infra;

partial class DbItem
{
    public DbValue GetFieldValueOrThrow(string fieldName)
    {
        var fieldIndex = GetFieldIndex(fieldName ?? string.Empty);

        if (fieldIndex is null)
        {
            throw new InvalidOperationException($"Field {fieldName} must be present in the data reader");
        }

        return GetDbValue(fieldIndex.Value);
    }
}
namespace GGroupp.Infra;

partial class DbItem
{
    public DbValue? GetFieldValueOrDefault(string fieldName)
    {
        var name = fieldName ?? string.Empty;
        var fieldIndex = GetFieldIndex(name);

        if (fieldIndex is null)
        {
            return null;
        }

        return GetDbValue(fieldIndex.Value, name);
    }
}
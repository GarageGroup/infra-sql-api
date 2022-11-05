namespace GGroupp.Infra;

partial class DbItem
{
    public DbValue? GetFieldValueOrDefault(string fieldName)
    {
        var fieldIndex = GetFieldIndex(fieldName ?? string.Empty);

        if (fieldIndex is null)
        {
            return null;
        }

        return GetDbValue(fieldIndex.Value);
    }
}
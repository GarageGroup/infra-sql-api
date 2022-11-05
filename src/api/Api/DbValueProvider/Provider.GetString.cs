namespace GGroupp.Infra;

partial class DbValueProvider
{
    public string? GetString()
        =>
        dbDataReader.GetString(fieldIndex);
}
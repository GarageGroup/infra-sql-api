namespace GGroupp.Infra;

partial class DbValueProvider
{
    public long GetInt64()
        =>
        dbDataReader.GetInt64(fieldIndex);
}
namespace GGroupp.Infra;

partial class DbValueProvider
{
    public int GetInt32()
        =>
        dbDataReader.GetInt32(fieldIndex);
}
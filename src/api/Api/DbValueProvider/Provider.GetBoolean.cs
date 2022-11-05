namespace GGroupp.Infra;

partial class DbValueProvider
{
    public bool GetBoolean()
        =>
        dbDataReader.GetBoolean(fieldIndex);
}
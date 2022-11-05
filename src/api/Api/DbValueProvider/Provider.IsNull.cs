namespace GGroupp.Infra;

partial class DbValueProvider
{
    public bool IsNull()
        =>
        dbDataReader.IsDBNull(fieldIndex);
}
namespace GGroupp.Infra;

partial class DbValueProvider
{
    public short GetInt16()
        =>
        dbDataReader.GetInt16(fieldIndex);
}
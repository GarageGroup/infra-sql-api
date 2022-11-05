namespace GGroupp.Infra;

partial class DbValueProvider
{
    public byte GetByte()
        =>
        dbDataReader.GetByte(fieldIndex);
}
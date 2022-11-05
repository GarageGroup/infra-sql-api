namespace GGroupp.Infra;

partial class DbValueProvider
{
    public float GetFloat()
        =>
        dbDataReader.GetFloat(fieldIndex);
}
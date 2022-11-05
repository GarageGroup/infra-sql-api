namespace GGroupp.Infra;

partial class DbValueProvider
{
    public double GetDouble()
        =>
        dbDataReader.GetDouble(fieldIndex);
}
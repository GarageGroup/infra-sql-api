namespace GGroupp.Infra;

partial class DbValueProvider
{
    public decimal GetDecimal()
        =>
        dbDataReader.GetDecimal(fieldIndex);
}
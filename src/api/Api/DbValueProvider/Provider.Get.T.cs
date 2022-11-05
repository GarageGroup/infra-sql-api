namespace GGroupp.Infra;

partial class DbValueProvider
{
    public T Get<T>() where T
        : notnull
        =>
        dbDataReader.GetFieldValue<T>(fieldIndex);
}
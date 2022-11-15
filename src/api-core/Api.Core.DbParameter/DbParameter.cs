namespace GGroupp.Infra;

public sealed record class DbParameter
{
    public DbParameter(string name, object value)
    {
        Name = name ?? string.Empty;
        Value = value;
    }

    public string Name { get; }

    public object? Value { get; }
}
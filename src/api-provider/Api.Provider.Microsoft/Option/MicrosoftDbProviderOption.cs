namespace GGroupp.Infra;

public sealed record class MicrosoftDbProviderOption
{
    public MicrosoftDbProviderOption(string connectionString)
        =>
        ConnectionString = connectionString ?? string.Empty;

    public string ConnectionString { get; }
}
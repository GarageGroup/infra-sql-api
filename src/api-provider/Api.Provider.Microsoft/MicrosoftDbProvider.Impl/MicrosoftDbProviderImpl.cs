namespace GGroupp.Infra;

internal sealed partial class MicrosoftDbProviderImpl : IDbProvider
{
    public static MicrosoftDbProviderImpl Create(string connectionString)
        =>
        new(connectionString ?? string.Empty);

    private readonly string connectionString;

    private MicrosoftDbProviderImpl(string connectionString)
        =>
        this.connectionString = connectionString;
}
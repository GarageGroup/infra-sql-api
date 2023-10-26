namespace GarageGroup.Infra;

internal sealed record class DbExtensionFieldMetadata
{
    public DbExtensionFieldMetadata(string propertyName)
        =>
        PropertyName = propertyName ?? string.Empty;

    public string PropertyName { get; }
}
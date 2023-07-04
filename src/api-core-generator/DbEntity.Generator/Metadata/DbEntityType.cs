namespace GarageGroup.Infra;

internal sealed record class DbEntityType
{
    public DbEntityType(DisplayedTypeData displayedData, bool isRecordType, bool isValueType)
    {
        DisplayedData = displayedData;
        IsRecordType = isRecordType;
        IsValueType = isValueType;
    }

    public DisplayedTypeData DisplayedData { get; }

    public bool IsRecordType { get; }

    public bool IsValueType { get; }
}
namespace GarageGroup.Infra;

public interface IDbItem
{
    DbValue GetFieldValueOrThrow(string fieldName);

    DbValue? GetFieldValueOrDefault(string fieldName);
}
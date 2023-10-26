using System.Collections.Generic;

namespace GarageGroup.Infra;

public interface IDbItem
{
    IEnumerable<string> Fields { get; }

    DbValue GetFieldValueOrThrow(string fieldName);

    DbValue? GetFieldValueOrDefault(string fieldName);
}
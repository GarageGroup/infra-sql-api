using System.Collections.Generic;
using System.Data.Common;

namespace GarageGroup.Infra;

internal sealed partial class DbItem : IDbItem
{
    private readonly DbDataReader dbDataReader;

    private readonly IReadOnlyDictionary<string, int> fieldIndexes;

    internal DbItem(DbDataReader dbDataReader, IReadOnlyDictionary<string, int> fieldIndexes)
    {
        this.dbDataReader = dbDataReader;
        this.fieldIndexes = fieldIndexes;
    }

    private int? GetFieldIndex(string fieldName)
        =>
        fieldIndexes.TryGetValue(fieldName, out var index) ? index : null;

    private DbValue GetDbValue(int fieldIndex, string fieldName)
        =>
        new(
            dbValueProvider: new DbValueProvider(dbDataReader, fieldIndex, fieldName));
}
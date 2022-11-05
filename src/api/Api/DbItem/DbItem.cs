using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace GGroupp.Infra;

internal sealed partial class DbItem : IDbItem
{
    private readonly DbDataReader dbDataReader;

    private readonly Lazy<IReadOnlyDictionary<string, int>> lazyFieldIndexes;

    internal DbItem(DbDataReader dbDataReader)
    {
        this.dbDataReader = dbDataReader;
        lazyFieldIndexes = new(CreateFieldIndexes);

        IReadOnlyDictionary<string, int> CreateFieldIndexes()
            =>
            Enumerable.Range(0, dbDataReader.FieldCount).ToDictionary(dbDataReader.GetName, static index => index);
    }

    private int? GetFieldIndex(string fieldName)
        =>
        lazyFieldIndexes.Value.TryGetValue(fieldName, out var index) ? index : null;

    private DbValue GetDbValue(int fieldIndex)
        =>
        new(
            dbValueProvider: new DbValueProvider(dbDataReader, fieldIndex));
}
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

internal sealed record class StubDbEntity : IDbEntity<StubDbEntity>
{
    public static StubDbEntity ReadEntity(IDbItem dbItem)
        =>
        new(
            dataReader: dbItem.GetInnerFieldValue<DbDataReader>("dbDataReader"),
            fieldIndexes: dbItem.GetInnerFieldValue<IReadOnlyDictionary<string, int>>("fieldIndexes"));

    internal StubDbEntity(DbDataReader? dataReader, IReadOnlyDictionary<string, int>? fieldIndexes)
    {
        DataReader = dataReader;
        FieldIndexes = fieldIndexes?.ToFlatArray() ?? default;
    }

    public DbDataReader? DataReader { get; }

    public FlatArray<KeyValuePair<string, int>> FieldIndexes { get; }
}
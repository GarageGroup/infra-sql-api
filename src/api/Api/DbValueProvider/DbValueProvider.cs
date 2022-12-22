using System;
using System.Data.Common;

namespace GGroupp.Infra;

internal sealed partial class DbValueProvider : IDbValueProvider
{
    private readonly DbDataReader dbDataReader;

    private readonly int fieldIndex;

    private readonly string fieldName;

    internal DbValueProvider(DbDataReader dbDataReader, int fieldIndex, string fieldName)
    {
        this.dbDataReader = dbDataReader;
        this.fieldIndex = fieldIndex;
        this.fieldName = fieldName ?? string.Empty;
    }

    private InvalidOperationException WrapSourceException(Exception sourceException)
        =>
        new($"An unexpected exception was thrown while getting a field '{fieldName}' value", sourceException);
}
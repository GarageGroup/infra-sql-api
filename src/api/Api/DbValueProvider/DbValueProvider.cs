using System.Data.Common;

namespace GGroupp.Infra;

internal sealed partial class DbValueProvider : IDbValueProvider
{
    private readonly DbDataReader dbDataReader;

    private readonly int fieldIndex;

    internal DbValueProvider(DbDataReader dbDataReader, int fieldIndex)
    {
        this.dbDataReader = dbDataReader;
        this.fieldIndex = fieldIndex;
    }
}
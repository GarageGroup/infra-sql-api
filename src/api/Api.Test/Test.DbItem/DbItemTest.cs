using System.Data.Common;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

public static partial class DbItemTest
{
    private static void VerifyInnerState(this DbValue actual, DbDataReader dbDataReader, int fieldIndex, string fieldName)
    {
        var actualDbValueProvider = actual.GetInnerFieldValue<IDbValueProvider>("dbValueProvider");
        Assert.NotNull(actualDbValueProvider);

        var actualDbDataReader = actualDbValueProvider.GetInnerFieldValue<DbDataReader>("dbDataReader");
        Assert.Same(dbDataReader, actualDbDataReader);

        var actualFieldIndex = actualDbValueProvider.GetInnerFieldValue<int>("fieldIndex");
        Assert.StrictEqual(fieldIndex, actualFieldIndex);

        var actualFieldName = actualDbValueProvider.GetInnerFieldValue<string>("fieldName");
        Assert.Equal(fieldName, actualFieldName);
    }
}
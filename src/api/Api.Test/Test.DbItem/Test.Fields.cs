using System.Collections.Generic;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class DbItemTest
{
    [Fact]
    public static void FieldsGet_FieldIndexesDictionaryIsEmpty_ExpectEmpty()
    {
        var fieldIndexes = new Dictionary<string, int>(0);

        using var dbDataReader = new StubDbDataReader(Mock.Of<IStubDbDataReader>());
        var dbItem = new DbItem(dbDataReader, fieldIndexes);

        var actual = dbItem.Fields;
        Assert.Empty(actual);
    }

    [Fact]
    public static void FieldsGet_FieldIndexesDictionaryIsNotEmpty_ExpectIndexesDictionaryKeys()
    {
        var fieldIndexes = new Dictionary<string, int>
        {
            ["First"] = 51,
            ["Second"] = 353,
            [string.Empty] = 52,
            ["Third"] = -11
        };

        using var dbDataReader = new StubDbDataReader(Mock.Of<IStubDbDataReader>());
        var dbItem = new DbItem(dbDataReader, fieldIndexes);

        var actual = dbItem.Fields;
        var expected = new[] { "First", "Second", string.Empty, "Third" };

        Assert.Equal(expected, actual);
    }
}
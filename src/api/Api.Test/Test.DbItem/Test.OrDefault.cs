using System.Collections.Generic;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

partial class DbItemTest
{
    [Fact]
    public static void GetFieldValueOrDefault_FieldNameIsOutOfIndexes_ExpectNull()
    {
        var fieldIndexes = new Dictionary<string, int>
        {
            ["SomeName"] = 721
        };

        using var dbDataReader = new StubDbDataReader(Mock.Of<IStubDbDataReader>());
        var dbItem = new DbItem(dbDataReader, fieldIndexes);

        var actual = dbItem.GetFieldValueOrDefault("someName");
        Assert.Null(actual);
    }

    [Fact]
    public static void GetFieldValueOrDefault_FieldNameIsInIndexes_ExpectCorrectValue()
    {
        var fieldIndexes = new Dictionary<string, int>
        {
            ["Some first name"] = 15,
            ["Some Name"] = 115
        };

        using var dbDataReader = new StubDbDataReader(Mock.Of<IStubDbDataReader>());
        var dbItem = new DbItem(dbDataReader, fieldIndexes);

        var actual = dbItem.GetFieldValueOrDefault("Some Name");

        Assert.NotNull(actual);
        actual.VerifyInnerState(dbDataReader, 115, "Some Name");
    }
}
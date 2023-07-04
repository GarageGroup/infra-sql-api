using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class DbItemTest
{
    [Fact]
    public static void GetFieldValueOrThrow_FieldNameIsOutOfIndexes_ExpectInvalidOperationException()
    {
        var fieldIndexes = new Dictionary<string, int>
        {
            ["SomeName"] = 21
        };

        using var dbDataReader = new StubDbDataReader(Mock.Of<IStubDbDataReader>());
        var dbItem = new DbItem(dbDataReader, fieldIndexes);

        var ex = Assert.Throws<InvalidOperationException>(Test);

        var expectedMessage = "Field 'someName' must be present in the data reader";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbItem.GetFieldValueOrThrow("someName");
    }

    [Fact]
    public static void GetFieldValueOrThrow_FieldNameIsInIndexes_ExpectCorrectValue()
    {
        var fieldIndexes = new Dictionary<string, int>
        {
            ["First"] = 81,
            ["Second"] = 64,
            ["Third"] = 87,
            ["Fourth"] = 253
        };

        using var dbDataReader = new StubDbDataReader(Mock.Of<IStubDbDataReader>());
        var dbItem = new DbItem(dbDataReader, fieldIndexes);

        var actual = dbItem.GetFieldValueOrThrow("Third");

        Assert.NotNull(actual);
        actual.VerifyInnerState(dbDataReader, 87, "Third");
    }
}
using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void GetDateOnly_ExpectFromDbDataReader()
    {
        object value = new DateOnly(2023, 01, 25);
        const int index = 26;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(db => db.GetValue(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "OtherName");
        var actual = dbValueProvider.GetDateOnly();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void GetDateOnly_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some error message");
        _ = mockDbDataReader.Setup(static db => db.GetValue(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 27, "SomeName");

        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'SomeName' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetDateOnly();
    }
}
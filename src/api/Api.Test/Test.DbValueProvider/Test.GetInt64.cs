using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void GetInt64_ExpectFromDbDataReader()
    {
        const long value = 1012387;
        const int index = 685;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(static db => db.GetInt64(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeName");
        var actual = dbValueProvider.GetInt64();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void GetInt64_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some exception message");
        _ = mockDbDataReader.Setup(static db => db.GetInt64(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 36, "SomeField");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'SomeField' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetInt64();
    }
}
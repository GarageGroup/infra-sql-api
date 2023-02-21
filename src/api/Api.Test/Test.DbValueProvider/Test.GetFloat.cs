using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void GetFloat_ExpectFromDbDataReader()
    {
        const float value = 561;
        const int index = 11;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(static db => db.GetFloat(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeField");
        var actual = dbValueProvider.GetFloat();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void GetFloat_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some exception message");
        _ = mockDbDataReader.Setup(static db => db.GetFloat(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 73, "SomeName");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'SomeName' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetFloat();
    }
}
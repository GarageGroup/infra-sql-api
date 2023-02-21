using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void GetInt16_ExpectFromDbDataReader()
    {
        const short value = -53;
        const int index = 64;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(static db => db.GetInt16(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeName");
        var actual = dbValueProvider.GetInt16();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void GetInt16_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some exception message");
        _ = mockDbDataReader.Setup(static db => db.GetInt16(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 81, "Some Field");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'Some Field' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetInt16();
    }
}
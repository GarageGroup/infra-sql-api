using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void GetDecimal_ExpectFromDbDataReader()
    {
        const decimal value = 78123.581m;
        const int index = 57;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(static db => db.GetDecimal(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeField");
        var actual = dbValueProvider.GetDecimal();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void GetDecimal_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some exception message");
        _ = mockDbDataReader.Setup(static db => db.GetDecimal(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 73, "SomeName");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'SomeName' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetDecimal();
    }
}
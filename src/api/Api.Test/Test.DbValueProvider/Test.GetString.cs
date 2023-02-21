using System;
using Moq;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Theory]
    [InlineData(null)]
    [InlineData(TestData.EmptyString)]
    [InlineData(TestData.SomeString)]
    public static void GetString_ExpectFromDbDataReader(string? value)
    {
        const int index = 563;
        var innerDbDataReader = Mock.Of<IStubDbDataReader>(db => db.GetString(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeName");
        var actual = dbValueProvider.GetString();

        Assert.Equal(value, actual);
    }

    [Fact]
    public static void GetString_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some exception message");
        _ = mockDbDataReader.Setup(static db => db.GetString(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 25, "Some name");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'Some name' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetString();
    }
}
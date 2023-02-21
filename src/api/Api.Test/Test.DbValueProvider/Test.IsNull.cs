using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public static void IsNull_ExpectFromDbDataReader(bool value)
    {
        const int index = 74;
        var innerDbDataReader = Mock.Of<IStubDbDataReader>(db => db.IsDBNull(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeName");
        var actual = dbValueProvider.IsNull();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void IsNull_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some exception message");
        _ = mockDbDataReader.Setup(static db => db.IsDBNull(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 23, "Some name");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'Some name' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.IsNull();
    }
}
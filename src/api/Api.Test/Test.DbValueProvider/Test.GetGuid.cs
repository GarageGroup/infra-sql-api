using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void GetGuid_ExpectFromDbDataReader()
    {
        var value = Guid.Parse("f9164d1f-adf6-49a0-baa7-454ac74f9ee4");
        const int index = 31;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(db => db.GetGuid(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeName");
        var actual = dbValueProvider.GetGuid();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void GetGuid_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some error message");
        _ = mockDbDataReader.Setup(static db => db.GetGuid(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 681, "SomeField");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'SomeField' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetGuid();
    }
}
using System;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void GetDateTime_ExpectFromDbDataReader()
    {
        var value = new DateTime(2022, 10, 16, 23, 03, 24);
        const int index = 5;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(db => db.GetDateTime(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeName");
        var actual = dbValueProvider.GetDateTime();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void GetDateTime_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some error message");
        _ = mockDbDataReader.Setup(static db => db.GetDateTime(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 681, "SomeField");

        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'SomeField' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetDateTime();
    }
}
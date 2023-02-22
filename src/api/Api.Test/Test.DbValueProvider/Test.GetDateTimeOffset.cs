using System;
using Moq;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void GetDateTimeOffset_ExpectFromDbDataReader()
    {
        object value = new DateTimeOffset(2017, 12, 28, 01, 45, 37, TimeSpan.FromHours(3));
        const int index = 75;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(db => db.GetValue(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "SomeField");
        var actual = dbValueProvider.GetDateTimeOffset();

        Assert.StrictEqual(value, actual);
    }

    [Fact]
    public static void GetDateTimeOffset_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some error message");
        _ = mockDbDataReader.Setup(static db => db.GetValue(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 695, "SomeName");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'SomeName' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.GetDateTimeOffset();
    }
}
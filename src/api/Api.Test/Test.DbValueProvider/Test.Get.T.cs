using System;
using Moq;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class DbValueProviderTest
{
    [Fact]
    public static void Get_T_ExpectFromDbDataReader()
    {
        var value = TestData.PlusFifteenIdRefType;
        const int index = 21;

        var innerDbDataReader = Mock.Of<IStubDbDataReader>(db => db.GetValue(index) == value);
        using var dbDataReader = new StubDbDataReader(innerDbDataReader);

        var dbValueProvider = new DbValueProvider(dbDataReader, index, "Some Name");
        var actual = dbValueProvider.Get<RefType>();

        Assert.Equal(value, actual);
    }

    [Fact]
    public static void Get_T_ExceptionIsThrown_ExpectInvalidOperationException()
    {
        var mockDbDataReader = new Mock<IStubDbDataReader>();

        var sourceException = new Exception("Some exception message");
        _ = mockDbDataReader.Setup(static db => db.GetValue(It.IsAny<int>())).Throws(sourceException);

        using var dbDataReader = new StubDbDataReader(mockDbDataReader.Object);
        var dbValueProvider = new DbValueProvider(dbDataReader, 11, "Some Name");
        
        var ex = Assert.Throws<InvalidOperationException>(Test);
        Assert.Same(sourceException, ex.InnerException);

        var expectedMessage = "An unexpected exception was thrown while getting a field 'Some Name' value";
        Assert.Equal(expectedMessage, ex.Message);

        void Test()
            =>
            _ = dbValueProvider.Get<RefType>();
    }
}
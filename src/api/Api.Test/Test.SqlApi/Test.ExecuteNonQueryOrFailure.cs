using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class SqlApiTest
{
    [Fact]
    public static async Task ExecuteNonQueryOrFailureAsync_QueryIsNull_ExpectArgumentNullException()
    {
        using var dbCommand = CreateDbCommand(35);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = new SqlApi(dbProvider);
        var cancellationToken = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAsync);
        Assert.Equal("query", ex.ParamName);

        async Task TestAsync()
            =>
            _ = await sqlApi.ExecuteNonQueryOrFailureAsync(null!, cancellationToken);
    }

    [Fact]
    public static void ExecuteNonQueryOrFailureAsync_CancellationTokenIsCanceled_ExpectCanceledValueTask()
    {
        using var dbCommand = CreateDbCommand(7);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = new SqlApi(dbProvider);

        var cancellationToken = new CancellationToken(canceled: true);
        var actual = sqlApi.ExecuteNonQueryOrFailureAsync(SomeDbQuery, cancellationToken);

        Assert.True(actual.IsCanceled);
    }

    [Fact]
    public static async Task ExecuteNonQueryOrFailureAsync_CancellationTokenIsNotCanceled_ExpectConnectionOpenCalledOnce()
    {
        using var dbCommand = CreateDbCommand(347);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await sqlApi.ExecuteNonQueryOrFailureAsync(SomeDbQuery, cancellationToken);

        mockDbConnection.Verify(static db => db.Open(), Times.Once);
    }

    [Fact]
    public static async Task ExecuteNonQueryOrFailureAsync_ConnectionThrowsException_ExpectFailure()
    {
        var dbConnectionException = new StubException("Some error message");

        var mockDbConnection = CreateMockDbConnection(dbConnectionException);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var actual = await sqlApi.ExecuteNonQueryOrFailureAsync(SomeDbQuery, default);
        var expected = Failure.Create("An unexpected exception was thrown when executing the input query", dbConnectionException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [InlineData(TestData.EmptyString)]
    [InlineData(TestData.SomeString)]
    public static async Task ExecuteNonQueryOrFailureAsync_ConnectionDoesNotThrowException_ExpectCommandTextIsSqlQuery(
        string sqlQuery)
    {
        using var dbCommand = CreateDbCommand(573);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var dbQuery = new StubDbQuery(
            query: sqlQuery,
            parameters: new DbParameter[]
            {
                new("SomeParameter", TestData.MinusOne)
            });

        _ = await sqlApi.ExecuteNonQueryOrFailureAsync(dbQuery, default);
        Assert.Equal(sqlQuery, dbCommand.CommandText);
    }

    [Fact]
    public static async Task ExecuteNonQueryOrFailureAsync_ConnectionDoesNotThrowException_ExpectCommandParametersAreDistinct()
    {
        using var dbCommand = CreateDbCommand(73);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var parameters = new Dictionary<DbParameter, object>
        {
            [new("FirstParam", 71)] = TestData.MixedWhiteSpacesString,
            [new("SecondParam", new())] = TestData.SomeString,
            [new(string.Empty, "Some text value")] = TestData.PlusFifteenIdRefType,
            [new("FirstParam", false)] = decimal.One,
            [new("FifthName", null)] = TestData.SomeTextRecordStruct
        };

        var dbProvider = CreateDbProvider(dbConnection, parameters);
        var sqlApi = new SqlApi(dbProvider);

        var dbQuery = new StubDbQuery(
            query: "SELECT * From Product",
            parameters: parameters.Select(GetKey).ToFlatArray());

        _ = await sqlApi.ExecuteNonQueryOrFailureAsync(dbQuery, default);
        var actual = dbCommand.Parameters.GetInnerFieldValue<List<object>>("parameters") ?? new();

        var expected = new object[]
        {
            decimal.One, TestData.SomeString, TestData.PlusFifteenIdRefType, TestData.SomeTextRecordStruct
        };

        Assert.Equal(expected, actual);

        static DbParameter GetKey(KeyValuePair<DbParameter, object> kv)
            =>
            kv.Key;
    }

    [Theory]
    [InlineData(TestData.MinusOne)]
    [InlineData(TestData.Zero)]
    [InlineData(TestData.PlusFifteen)]
    public static async Task ExecuteNonQueryOrFailureAsync_ConnectionDoesNotThrowExceptionAndTimeoutIsNotNull_ExpectCommandTimeoutWasConfigured(
        int timeout)
    {
        using var dbCommand = CreateDbCommand(73);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var dbQuery = new StubDbQuery(
            query: "SELECT * From Product",
            parameters: new DbParameter[]
            {
                new("SomeParameterName", TestData.PlusFifteenIdRefType)
            })
        {
            TimeoutInSeconds = timeout
        };

        _ = await sqlApi.ExecuteNonQueryOrFailureAsync(dbQuery, default);
        Assert.Equal(timeout, dbCommand.CommandTimeout);
    }

    [Fact]
    public static async Task ExecuteNonQueryOrFailureAsync_CommandThrowsException_ExpectFailure()
    {
        var dbCommandException = new InvalidOperationException("Some Exception Message");
        using var dbCommand = CreateDbCommand(dbCommandException);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var actual = await sqlApi.ExecuteNonQueryOrFailureAsync(SomeDbQuery, default);
        var expected = Failure.Create("An unexpected exception was thrown when executing the input query", dbCommandException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [InlineData(TestData.MinusFifteen)]
    [InlineData(TestData.Zero)]
    [InlineData(int.MaxValue)]
    public static async Task ExecuteNonQueryOrFailureAsync_CommandDoesNotThrowException_ExpectSuccessResult(
        int nonQueryResult)
    {
        using var dbCommand = CreateDbCommand(nonQueryResult);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var actual = await sqlApi.ExecuteNonQueryOrFailureAsync(SomeDbQuery, default);
        var expected = nonQueryResult;

        Assert.StrictEqual(expected, actual);
    }
}
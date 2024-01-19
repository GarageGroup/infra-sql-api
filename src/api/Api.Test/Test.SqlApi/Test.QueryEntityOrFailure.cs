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
    public static async Task QueryEntityOrFailureAsync_QueryIsNull_ExpectArgumentNullException()
    {
        using var dbDataReader = CreateDbDataReader(3, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = new SqlApi(dbProvider);
        var cancellationToken = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAsync);
        Assert.Equal("query", ex.ParamName);

        async Task TestAsync()
            =>
            _ = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(null!, cancellationToken);
    }

    [Fact]
    public static void QueryEntityOrFailureAsync_CancellationTokenIsCanceled_ExpectCanceledValueTask()
    {
        using var dbDataReader = CreateDbDataReader(5, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = new SqlApi(dbProvider);
        var cancellationToken = new CancellationToken(canceled: true);

        var actual = sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, cancellationToken);
        Assert.True(actual.IsCanceled);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_CancellationTokenIsNotCanceled_ExpectConnectionOpenCalledOnce()
    {
        using var dbDataReader = CreateDbDataReader(7, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        _ = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);
        mockDbConnection.Verify(static db => db.Open(), Times.Once);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_ConnectionThrowsException_ExpectUnknownFailure()
    {
        var dbConnectionException = new Exception("Some error message");

        var mockDbConnection = CreateMockDbConnection(dbConnectionException);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var actual = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);
        var expected = Failure.Create(
            EntityQueryFailureCode.Unknown,
            "An unexpected exception was thrown when executing the input query",
            dbConnectionException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [InlineData(TestData.EmptyString)]
    [InlineData(TestData.SomeString)]
    public static async Task QueryEntityOrFailureAsync_ConnectionDoesNotThrowException_ExpectCommandTextIsSqlQuery(
        string sqlQuery)
    {
        using var dbDataReader = CreateDbDataReader(5, "Field01", "Field02");
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var dbQuery = new StubDbQuery(
            query: sqlQuery,
            parameters: new DbParameter[]
            {
                new("Param01", null),
                new("Param03", TestData.PlusFifteenIdRefType)
            });

        _ = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(dbQuery, default);
        Assert.Equal(sqlQuery, dbCommand.CommandText);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_ConnectionDoesNotThrowException_ExpectCommandParametersAreDistinct()
    {
        using var dbDataReader = CreateDbDataReader(3, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var parameters = new Dictionary<DbParameter, object>
        {
            [new(string.Empty, new())] = TestData.PlusFifteen,
            [new("Second", string.Empty)] = TestData.SomeString,
            [new("Third", 'a')] = TestData.MinusFifteenIdRefType,
            [new("Second", byte.MaxValue)] = TestData.ZeroIdNullNameRecord,
            [new("Fifth", decimal.MinusOne)] = TestData.SomeTextRecordStruct
        };

        var dbProvider = CreateDbProvider(dbConnection, parameters);
        var sqlApi = new SqlApi(dbProvider);

        var dbQuery = new StubDbQuery(
            query: "Some SQL",
            parameters: parameters.Select(GetKey).ToFlatArray());

        _ = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(dbQuery, default);
        var actual = dbCommand.Parameters.GetInnerFieldValue<List<object>>("parameters") ?? [];

        var expected = new object[]
        {
            TestData.PlusFifteen, TestData.ZeroIdNullNameRecord, TestData.MinusFifteenIdRefType, TestData.SomeTextRecordStruct
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
    public static async Task QueryEntityOrFailureAsync_ConnectionDoesNotThrowExceptionAndTimeoutIsNotNull_ExpectCommandTimeoutWasConfigured(
        int timeout)
    {
        using var dbDataReader = CreateDbDataReader(0, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var dbQuery = new StubDbQuery(
            query: "SELECT * From Product",
            parameters: default)
        {
            TimeoutInSeconds = timeout
        };

        _ = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(dbQuery, default);
        Assert.Equal(timeout, dbCommand.CommandTimeout);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_CommandThrowsException_ExpectUnknownFailure()
    {
        var dbCommandException = new StubException("Some Exception Message");
        using var dbCommand = CreateDbCommand(dbCommandException);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var actual = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);
        var expected = Failure.Create(
            EntityQueryFailureCode.Unknown,
            "An unexpected exception was thrown when executing the input query",
            dbCommandException);

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_DbDataReaderIsEmpty_ExpectAbsent()
    {
        using var dbDataReader = CreateDbDataReader(0, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var actual = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);
        var expected = Failure.Create(EntityQueryFailureCode.NotFound, "A db entity was not found by the input query");

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_DbDataReaderIsNotEmpty_ExpectSuccessFirstResult()
    {
        using var dbDataReader = CreateDbDataReader(3, "FirstField", "SecondField");
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider);

        var actual = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);

        var expectedFieldIndexes = new Dictionary<string, int>
        {
            ["FirstField"] = 0,
            ["SecondField"] = 1
        };
        var expected = new StubDbEntity(dbDataReader, expectedFieldIndexes);

        Assert.StrictEqual(expected, actual);
    }
}
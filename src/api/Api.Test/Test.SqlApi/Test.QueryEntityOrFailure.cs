using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Moq;
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

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

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

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);

        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);
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

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        _ = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);
        mockDbConnection.Verify(static db => db.Open(), Times.Once);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_ConnectionThrowsException_ExpectUnknownFailure()
    {
        var dbConnectionException = new Exception("Some error message");

        var mockDbConnection = CreateMockDbConnection(dbConnectionException);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        using var dbDataReader = CreateDbDataReader(7, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);

        var expected = Failure.Create(
            EntityQueryFailureCode.Unknown,
            "An unexpected exception was thrown when executing the input database query",
            dbConnectionException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(SqlApiTestSource.DbCommandTestData), MemberType = typeof(SqlApiTestSource))]
    internal static async Task QueryEntityOrFailureAsync_ConnectionDoesNotThrowException_ExpectDbCommandGetCalledOnce(
        StubDbQuery dbQuery, StubDbCommandRequest expectedRequest)
    {
        using var dbDataReader = CreateDbDataReader(5, "Field01", "Field02");
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand, OnCommandGet);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        _ = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(dbQuery, default);

        mockDbProvider.Verify(
            p => p.GetDbCommand(dbConnection, expectedRequest.CommandText, It.IsAny<IReadOnlyCollection<DbParameter>?>(), expectedRequest.Timeout),
            Times.Once);

        void OnCommandGet(IReadOnlyCollection<DbParameter>? actual)
            =>
            Assert.Equal(expectedRequest.Parameters, actual);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_CommandThrowsException_ExpectUnknownFailure()
    {
        var dbCommandException = new StubException("Some Exception Message");
        using var dbCommand = CreateDbCommand(dbCommandException);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);
        var expected = Failure.Create(
            EntityQueryFailureCode.Unknown,
            "An unexpected exception was thrown when executing the input database query",
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

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.QueryEntityOrFailureAsync<StubDbEntity>(SomeDbQuery, default);
        var expected = Failure.Create(EntityQueryFailureCode.NotFound, "A db entity was not found by the input database query");

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task QueryEntityOrFailureAsync_DbDataReaderIsNotEmpty_ExpectSuccessFirstResult()
    {
        using var dbDataReader = CreateDbDataReader(3, "FirstField", "SecondField");
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

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
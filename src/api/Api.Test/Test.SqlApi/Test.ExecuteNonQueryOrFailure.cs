using System;
using System.Collections.Generic;
using System.Data.Common;
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

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);

        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);
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

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);

        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

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

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

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

        using var dbCommand = CreateDbCommand(347);
        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);

        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.ExecuteNonQueryOrFailureAsync(SomeDbQuery, default);
        var expected = Failure.Create("An unexpected exception was thrown when executing the input database query", dbConnectionException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [MemberData(nameof(SqlApiTestSource.DbCommandTestData), MemberType = typeof(SqlApiTestSource))]
    internal static async Task ExecuteNonQueryOrFailureAsync_ConnectionDoesNotThrowException_ExpectDbCommandGetCalledOnce(
        StubDbQuery dbQuery, StubDbCommandRequest expectedRequest)
    {
        using var dbCommand = CreateDbCommand(573);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand, OnCommandGet);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        _ = await sqlApi.ExecuteNonQueryOrFailureAsync(dbQuery, default);

        mockDbProvider.Verify(
            p => p.GetDbCommand(dbConnection, expectedRequest.CommandText, It.IsAny<IReadOnlyCollection<DbParameter>?>(), expectedRequest.Timeout),
            Times.Once);

        void OnCommandGet(IReadOnlyCollection<DbParameter>? actual)
            =>
            Assert.Equal(expectedRequest.Parameters, actual);
    }

    [Fact]
    public static async Task ExecuteNonQueryOrFailureAsync_CommandThrowsException_ExpectFailure()
    {
        var dbCommandException = new InvalidOperationException("Some Exception Message");
        using var dbCommand = CreateDbCommand(dbCommandException);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.ExecuteNonQueryOrFailureAsync(SomeDbQuery, default);
        var expected = Failure.Create("An unexpected exception was thrown when executing the input database query", dbCommandException);

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

        var mockDbProvider = CreateMockDbProvider(dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.ExecuteNonQueryOrFailureAsync(SomeDbQuery, default);
        var expected = nonQueryResult;

        Assert.StrictEqual(expected, actual);
    }
}
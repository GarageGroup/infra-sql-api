using System;
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
    public static void PingAsync_CancellationTokenIsCanceled_ExpectCanceledValueTask()
    {
        using var dbCommand = CreateDbCommand(71);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.TransactSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var cancellationToken = new CancellationToken(canceled: true);
        var actual = sqlApi.PingAsync(default, cancellationToken);

        Assert.True(actual.IsCanceled);
    }

    [Fact]
    public static async Task PingAsync_CancellationTokenIsNotCanceled_ExpectConnectionOpenCalledOnce()
    {
        using var dbCommand = CreateDbCommand(19);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.TransactSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var cancellationToken = new CancellationToken(canceled: false);
        _ = await sqlApi.PingAsync(default, cancellationToken);

        mockDbConnection.Verify(static db => db.Open(), Times.Once);
    }

    [Fact]
    public static async Task PingAsync_ConnectionThrowsException_ExpectFailure()
    {
        var dbConnectionException = new StubException("Some exception message");

        var mockDbConnection = CreateMockDbConnection(dbConnectionException);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        using var dbCommand = CreateDbCommand(57);
        var mockDbProvider = CreateMockDbProvider(SqlDialect.TransactSql, dbConnection, dbCommand);

        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.PingAsync(default, default);
        var expected = Failure.Create("An unexpected exception was thrown when trying to ping a database", dbConnectionException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [InlineData(SqlDialect.TransactSql)]
    [InlineData(SqlDialect.PostgreSql)]
    [InlineData((SqlDialect)371)]
    public static async Task PingAsync_ConnectionDoesNotThrowException_ExpectDbCommandGetCalledOnce(
        SqlDialect dialect)
    {
        using var dbCommand = CreateDbCommand(10);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(dialect, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        _ = await sqlApi.PingAsync(default, default);
        mockDbProvider.Verify(p => p.GetDbCommand(dbConnection, "SELECT 1;", null, null), Times.Once);
    }

    [Fact]
    public static async Task PingAsync_CommandThrowsException_ExpectFailure()
    {
        var dbCommandException = new InvalidOperationException("Some error Message");
        using var dbCommand = CreateDbCommand(dbCommandException);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.TransactSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.PingAsync(default, default);
        var expected = Failure.Create("An unexpected exception was thrown when trying to ping a database", dbCommandException);

        Assert.StrictEqual(expected, actual);
    }

    [Theory]
    [InlineData(TestData.MinusFifteen)]
    [InlineData(TestData.Zero)]
    [InlineData(int.MaxValue)]
    public static async Task PingAsync_CommandDoesNotThrowException_ExpectSuccessResult(
        int nonQueryResult)
    {
        using var dbCommand = CreateDbCommand(nonQueryResult);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.TransactSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.PingAsync(default, default);
        var expected = Result.Success<Unit>(default);

        Assert.StrictEqual(expected, actual);
    }
}
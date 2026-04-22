using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Moq;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class SqlApiTest
{
    [Fact]
    public static async Task ExecuteNonQueryAsync_QueryIsNull_ExpectArgumentNullException()
    {
        using var dbCommand = CreateDbCommand(53);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.TransactSql, dbConnection, dbCommand);

        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAsync);
        Assert.Equal("query", ex.ParamName);

        async Task TestAsync()
            =>
            _ = await sqlApi.ExecuteNonQueryAsync(null!, TestContext.Current.CancellationToken);
    }

    [Fact]
    public static async Task ExecuteNonQueryAsync_ExpectConnectionOpenCalledOnce()
    {
        using var dbCommand = CreateDbCommand(21);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.TransactSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        _ = await sqlApi.ExecuteNonQueryAsync(SomeDbQuery, TestContext.Current.CancellationToken);
        mockDbConnection.Verify(static db => db.Open(), Times.Once);
    }

    [Theory]
    [MemberData(nameof(SqlApiTestSource.DbCommandTestData), MemberType = typeof(SqlApiTestSource))]
    internal static async Task ExecuteNonQueryAsync_ExpectDbCommandGetCalledOnce(
        StubDbQuery dbQuery, SqlDialect dialect, StubDbCommandRequest expectedRequest)
    {
        using var dbCommand = CreateDbCommand(73);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(dialect, dbConnection, dbCommand, OnCommandGet);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        _ = await sqlApi.ExecuteNonQueryAsync(dbQuery, TestContext.Current.CancellationToken);

        mockDbProvider.Verify(
            p => p.GetDbCommand(dbConnection, expectedRequest.CommandText, It.IsAny<IReadOnlyCollection<DbParameter>?>(), expectedRequest.Timeout),
            Times.Once);

        void OnCommandGet(IReadOnlyCollection<DbParameter>? actual)
            =>
            Assert.Equal(expectedRequest.Parameters, actual);
    }

    [Theory]
    [InlineData(TestData.MinusFifteen)]
    [InlineData(TestData.Zero)]
    [InlineData(int.MaxValue)]
    public static async Task ExecuteNonQueryAsync_ExpectNonQueryResult(
        int nonQueryResult)
    {
        using var dbCommand = CreateDbCommand(nonQueryResult);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.PostgreSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.ExecuteNonQueryAsync(SomeDbQuery, TestContext.Current.CancellationToken);
        Assert.StrictEqual(nonQueryResult, actual);
    }
}
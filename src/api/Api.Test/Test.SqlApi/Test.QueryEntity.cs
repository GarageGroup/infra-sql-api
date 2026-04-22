using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class SqlApiTest
{
    [Fact]
    public static async Task QueryEntityOrAbsentAsync_QueryIsNull_ExpectArgumentNullException()
    {
        using var dbDataReader = CreateDbDataReader(3, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.PostgreSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAsync);

        Assert.Equal("query", ex.ParamName);

        async Task TestAsync()
            =>
            _ = await sqlApi.QueryEntityOrAbsentAsync<StubDbEntity>(null!, TestContext.Current.CancellationToken);
    }

    [Fact]
    public static async Task QueryEntityOrAbsentAsync_ExpectConnectionOpenCalledOnce()
    {
        using var dbDataReader = CreateDbDataReader(7, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.PostgreSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        _ = await sqlApi.QueryEntityOrAbsentAsync<StubDbEntity>(SomeDbQuery, TestContext.Current.CancellationToken);
        mockDbConnection.Verify(static db => db.Open(), Times.Once);
    }

    [Theory]
    [MemberData(nameof(SqlApiTestSource.DbCommandTestData), MemberType = typeof(SqlApiTestSource))]
    internal static async Task QueryEntityOrAbsentAsync_ExpectDbCommandGetCalledOnce(
        StubDbQuery dbQuery, SqlDialect dialect, StubDbCommandRequest expectedRequest)
    {
        using var dbDataReader = CreateDbDataReader(5, "Field01", "Field02");
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(dialect, dbConnection, dbCommand, OnCommandGet);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        _ = await sqlApi.QueryEntityOrAbsentAsync<StubDbEntity>(dbQuery, TestContext.Current.CancellationToken);

        mockDbProvider.Verify(
            p => p.GetDbCommand(dbConnection, expectedRequest.CommandText, It.IsAny<IReadOnlyCollection<DbParameter>?>(), expectedRequest.Timeout),
            Times.Once);

        void OnCommandGet(IReadOnlyCollection<DbParameter>? actual)
            =>
            Assert.Equal(expectedRequest.Parameters, actual);
    }

    [Fact]
    public static async Task QueryEntityOrAbsentAsync_DbDataReaderIsEmpty_ExpectAbsent()
    {
        using var dbDataReader = CreateDbDataReader(0, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.PostgreSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.QueryEntityOrAbsentAsync<StubDbEntity>(SomeDbQuery, TestContext.Current.CancellationToken);
        var expected = Result.Absent<StubDbEntity>();

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task QueryEntityOrAbsentAsync_DbDataReaderIsNotEmpty_ExpectSuccessFirstResult()
    {
        using var dbDataReader = CreateDbDataReader(3, "FirstField", "SecondField", "FirstField");
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var mockDbProvider = CreateMockDbProvider(SqlDialect.PostgreSql, dbConnection, dbCommand);
        var sqlApi = new SqlApi<DbConnection>(mockDbProvider.Object);

        var actual = await sqlApi.QueryEntityOrAbsentAsync<StubDbEntity>(SomeDbQuery, TestContext.Current.CancellationToken);

        var expectedFieldIndexes = new Dictionary<string, int>
        {
            ["FirstField"] = 0,
            ["SecondField"] = 1
        };
        var expected = new StubDbEntity(dbDataReader, expectedFieldIndexes);

        Assert.StrictEqual(expected, actual);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

partial class SqlApiTest
{
    [Fact]
    public static async Task ExecuteNonQueryAsync_QueryIsNull_ExpectArgumentNullException()
    {
        using var dbCommand = CreateDbCommand(53);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = new SqlApi(dbProvider, null);
        var cancellationToken = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAsync);
        Assert.Equal("query", ex.ParamName);

        async Task TestAsync()
            =>
            _ = await sqlApi.ExecuteNonQueryAsync(null!, cancellationToken);
    }

    [Fact]
    public static void ExecuteNonQueryAsync_CancellationTokenIsCanceled_ExpectCanceledValueTask()
    {
        using var dbCommand = CreateDbCommand(21);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = new SqlApi(dbProvider, null);

        var cancellationToken = new CancellationToken(canceled: true);
        var actual = sqlApi.ExecuteNonQueryAsync(SomeDbQuery, cancellationToken);

        Assert.True(actual.IsCanceled);
    }

    [Fact]
    public static async Task ExecuteNonQueryAsync_CancellationTokenIsNotCanceled_ExpectConnectionOpenCalledOnce()
    {
        using var dbCommand = CreateDbCommand(21);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider, null);

        _ = await sqlApi.ExecuteNonQueryAsync(SomeDbQuery, default);
        mockDbConnection.Verify(static db => db.Open());
    }

    [Theory]
    [InlineData(TestData.EmptyString)]
    [InlineData(TestData.SomeString)]
    public static async Task ExecuteNonQueryAsync_CancellationTokenIsNotCanceled_ExpectCommandTextIsSqlQuery(
        string sqlQuery)
    {
        using var dbCommand = CreateDbCommand(73);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider, null);

        var dbQuery = new StubDbQuery(
            query: sqlQuery,
            parameters: new DbParameter[]
            {
                new("SomeParameterName", TestData.PlusFifteenIdRefType)
            });

        _ = await sqlApi.ExecuteNonQueryAsync(dbQuery, default);
        Assert.Equal(sqlQuery, dbCommand.CommandText);
    }

    [Fact]
    public static async Task ExecuteNonQueryAsync_CancellationTokenIsNotCanceled_ExpectCommandParametersAreDistinct()
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
        var sqlApi = new SqlApi(dbProvider, null);

        var dbQuery = new StubDbQuery(
            query: "SELECT * From Product",
            parameters: parameters.Select(GetKey).ToFlatArray());

        _ = await sqlApi.ExecuteNonQueryAsync(dbQuery, default);
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
    public static async Task ExecuteNonQueryAsync_TimeoutIsNotNull_ExpectCommandTimeoutWasConfigured(
        int timeout)
    {
        using var dbCommand = CreateDbCommand(73);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider, null);

        var dbQuery = new StubDbQuery(
            query: "SELECT * From Product",
            parameters: new DbParameter[]
            {
                new("SomeParameterName", TestData.PlusFifteenIdRefType)
            })
        {
            TimeoutInSeconds = timeout
        };

        _ = await sqlApi.ExecuteNonQueryAsync(dbQuery, default);
        Assert.Equal(timeout, dbCommand.CommandTimeout);
    }

    [Theory]
    [InlineData(TestData.MinusFifteen)]
    [InlineData(TestData.Zero)]
    [InlineData(int.MaxValue)]
    public static async Task ExecuteNonQueryAsync_CancellationTokenIsNotCanceled_ExpectNonQueryResult(
        int nonQueryResult)
    {
        using var dbCommand = CreateDbCommand(nonQueryResult);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = new SqlApi(dbProvider, null);

        var actual = await sqlApi.ExecuteNonQueryAsync(SomeDbQuery, default);
        Assert.StrictEqual(nonQueryResult, actual);
    }
}
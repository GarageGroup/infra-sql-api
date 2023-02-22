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
    public static async Task QueryEntityOrAbsentAsync_QueryIsNull_ExpectArgumentNullException()
    {
        using var dbDataReader = CreateDbDataReader(3, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = SqlApi.Create(dbProvider);
        var cancellationToken = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAsync);
        Assert.Equal("query", ex.ParamName);

        async Task TestAsync()
            =>
            _ = await sqlApi.QueryStubDbEntityOrAbsentAsync(null!, cancellationToken);
    }
#if NET6_0

    [Fact]
    public static async Task QueryEntityOrAbsentAsync_MapperIsNull_ExpectArgumentNullException()
    {
        using var dbDataReader = CreateDbDataReader(3, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = SqlApi.Create(dbProvider);
        var cancellationToken = new CancellationToken(canceled: false);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(TestAsync);
        Assert.Equal("mapper", ex.ParamName);

        async Task TestAsync()
            =>
            _ = await sqlApi.QueryEntityOrAbsentAsync<StubDbEntity>(SomeDbQuery, null!, cancellationToken);
    }
#endif

    [Fact]
    public static void QueryEntityOrAbsentAsync_CancellationTokenIsCanceled_ExpectCanceledValueTask()
    {
        using var dbDataReader = CreateDbDataReader(5, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);

        var sqlApi = SqlApi.Create(dbProvider);
        var cancellationToken = new CancellationToken(canceled: true);

        var actual = sqlApi.QueryStubDbEntityOrAbsentAsync(SomeDbQuery, cancellationToken);
        Assert.True(actual.IsCanceled);
    }

    [Fact]
    public static async Task QueryEntityOrAbsentAsync_CancellationTokenIsNotCanceled_ExpectConnectionOpenCalledOnce()
    {
        using var dbDataReader = CreateDbDataReader(7, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = SqlApi.Create(dbProvider);

        _ = await sqlApi.QueryStubDbEntityOrAbsentAsync(SomeDbQuery, default);
        mockDbConnection.Verify(static db => db.Open());
    }

    [Theory]
    [InlineData(TestData.EmptyString)]
    [InlineData(TestData.SomeString)]
    public static async Task QueryEntityOrAbsentAsync_CancellationTokenIsNotCanceled_ExpectCommandTextIsSqlQuery(
        string sqlQuery)
    {
        using var dbDataReader = CreateDbDataReader(5, "Field01", "Field02");
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = SqlApi.Create(dbProvider);

        var dbQuery = new StubDbQuery(
            query: sqlQuery,
            parameters: new DbParameter[]
            {
                new("Param01", null),
                new("Param03", TestData.PlusFifteenIdRefType)
            });

        _ = await sqlApi.QueryStubDbEntityOrAbsentAsync(dbQuery, default);
        Assert.Equal(sqlQuery, dbCommand.CommandText);
    }

    [Fact]
    public static async Task QueryEntityOrAbsentAsync_CancellationTokenIsNotCanceled_ExpectCommandParametersAreDistinct()
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
        var sqlApi = SqlApi.Create(dbProvider);

        var dbQuery = new StubDbQuery(
            query: "Some SQL",
            parameters: parameters.Select(GetKey).ToFlatArray());

        _ = await sqlApi.QueryStubDbEntityOrAbsentAsync(dbQuery, default);
        var actual = dbCommand.Parameters.GetInnerFieldValue<List<object>>("parameters") ?? new();

        var expected = new object[]
        {
            TestData.PlusFifteen, TestData.ZeroIdNullNameRecord, TestData.MinusFifteenIdRefType, TestData.SomeTextRecordStruct
        };

        Assert.Equal(expected, actual);

        static DbParameter GetKey(KeyValuePair<DbParameter, object> kv)
            =>
            kv.Key;
    }

    [Fact]
    public static async Task QueryEntityOrAbsentAsync_DbDataReaderIsEmpty_ExpectAbsent()
    {
        using var dbDataReader = CreateDbDataReader(0, SomeFieldNames);
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = SqlApi.Create(dbProvider);

        var actual = await sqlApi.QueryStubDbEntityOrAbsentAsync(SomeDbQuery, default);
        var expected = Result.Absent<StubDbEntity>();

        Assert.StrictEqual(expected, actual);
    }

    [Fact]
    public static async Task QueryEntityOrAbsentAsync_DbDataReaderIsNotEmpty_ExpectSuccessFirstResult()
    {
        using var dbDataReader = CreateDbDataReader(3, "FirstField", "SecondField");
        using var dbCommand = CreateDbCommand(dbDataReader);

        var mockDbConnection = CreateMockDbConnection(dbCommand);
        using var dbConnection = new StubDbConnection(mockDbConnection.Object);

        var dbProvider = CreateDbProvider(dbConnection);
        var sqlApi = SqlApi.Create(dbProvider);

        var actual = await sqlApi.QueryStubDbEntityOrAbsentAsync(SomeDbQuery, default);

        var expectedFieldIndexes = new Dictionary<string, int>
        {
            ["FirstField"] = 0,
            ["SecondField"] = 1
        };
        var expected = new StubDbEntity(dbDataReader, expectedFieldIndexes);

        Assert.StrictEqual(expected, actual);
    }
}
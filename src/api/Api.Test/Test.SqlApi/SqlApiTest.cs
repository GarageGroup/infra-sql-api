using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

public static partial class SqlApiTest
{
    private static readonly StubDbQuery SomeDbQuery
        =
        new(
            query: "Some SQL",
            parameters: new DbParameter[]
            {
                new("First", 3781.5m),
                new("Second", "")
            });

    private static readonly string[] SomeFieldNames
        =
        new[]
        {
            "First", "Second", "Third"
        };

    private static readonly Mock<ILoggerFactory> SomeLoggerFactory
        =
        new Mock<ILoggerFactory>();

    private static StubDbCommand CreateDbCommand(int nonQueryResult)
        =>
        new(
            Mock.Of<IStubDbCommand>(db => db.ExecuteNonQuery() == nonQueryResult));

    private static StubDbCommand CreateDbCommand(DbDataReader dbDataReader)
        =>
        new(
            Mock.Of<IStubDbCommand>(db => db.ExecuteDbDataReader(CommandBehavior.Default) == dbDataReader));

    private static StubDbDataReader CreateDbDataReader(int readCount, params string[] fieldNames)
    {
        var mock = new Mock<IStubDbDataReader>();

        _ = mock.SetupGet(static db => db.FieldCount).Returns(fieldNames.Length);
        _ = mock.Setup(static db => db.GetName(It.IsAny<int>())).Returns(GetFieldName);

        var remain = readCount;
        _ = mock.Setup(static db => db.Read()).Returns(Read);

        return new(mock.Object);

        string GetFieldName(int index)
            =>
            fieldNames[index];

        bool Read()
            =>
            remain > 0 && --remain >= 0;
    }

    private static Mock<IStubDbConnection> CreateMockDbConnection(DbCommand dbCommand)
    {
        var mock = new Mock<IStubDbConnection>();

        _ = mock.Setup(db => db.CreateDbCommand()).Returns(dbCommand);

        return mock;
    }

    private static IDbProvider CreateDbProvider(DbConnection dbConnection, IReadOnlyDictionary<DbParameter, object>? parameters = null)
    {
        var mock = new Mock<IDbProvider>();

        _ = mock.Setup(db => db.GetDbConnection()).Returns(dbConnection);
        
        if (parameters is null)
        {
            _ = mock.Setup(db => db.GetSqlParameter(It.IsAny<DbParameter>())).Returns(new object());
        }
        else
        {
            _ = mock.Setup(db => db.GetSqlParameter(It.IsAny<DbParameter>())).Returns(GetValue);
        }

        return mock.Object;

        object GetValue(DbParameter dbParameter)
            =>
            parameters[dbParameter];
    }

    private static ValueTask<FlatArray<StubDbEntity>> QueryStubDbEntitySetAsync(
        this ISqlQueryEntitySetSupplier sqlApi, IDbQuery dbQuery, CancellationToken cancellationToken)
        =>
#if NET7_0_OR_GREATER
        sqlApi.QueryEntitySetAsync<StubDbEntity>(dbQuery, cancellationToken);
#else
        sqlApi.QueryEntitySetAsync(dbQuery, StubDbEntity.ReadEntity, cancellationToken);
#endif

    private static ValueTask<Result<StubDbEntity, Unit>> QueryStubDbEntityOrAbsentAsync(
        this ISqlQueryEntitySupplier sqlApi, IDbQuery dbQuery, CancellationToken cancellationToken)
        =>
#if NET7_0_OR_GREATER
        sqlApi.QueryEntityOrAbsentAsync<StubDbEntity>(dbQuery, cancellationToken);
#else
        sqlApi.QueryEntityOrAbsentAsync(dbQuery, StubDbEntity.ReadEntity, cancellationToken);
#endif
}
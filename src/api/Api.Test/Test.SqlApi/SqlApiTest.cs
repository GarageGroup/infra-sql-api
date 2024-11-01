using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Moq;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

public static partial class SqlApiTest
{
    private static readonly StubDbQuery SomeDbQuery
        =
        new(
            queries: new Dictionary<SqlDialect, string>
            {
                [SqlDialect.TransactSql] = "Some TransactSql",
                [SqlDialect.PostgreSql] = "Some PostgreSql"
            },
            parameters:
            [
                new("First", 3781.5m),
                new("Second", "Some text")
            ]);

    private static readonly string[] SomeFieldNames
        =
        [
            "First", "Second", "Third"
        ];

    private static StubDbCommand CreateDbCommand(int nonQueryResult)
        =>
        new(
            Mock.Of<IStubDbCommand>(db => db.ExecuteNonQuery() == nonQueryResult));

    private static StubDbCommand CreateDbCommand(DbDataReader dbDataReader)
        =>
        new(
            Mock.Of<IStubDbCommand>(db => db.ExecuteDbDataReader(CommandBehavior.Default) == dbDataReader));

    private static StubDbCommand CreateDbCommand(Exception exception)
    {
        var mock = new Mock<IStubDbCommand>();

        _ = mock.Setup(c => c.ExecuteNonQuery()).Throws(exception);
        _ = mock.Setup(c => c.ExecuteDbDataReader(It.IsAny<CommandBehavior>())).Throws(exception);

        return new(mock.Object);
    }

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

    private static Mock<IStubDbConnection> CreateMockDbConnection(Exception exception)
    {
        var mock = new Mock<IStubDbConnection>();

        _ = mock.Setup(db => db.Open()).Throws(exception);

        return mock;
    }

    private static Mock<IDbProvider<DbConnection>> CreateMockDbProvider(
        SqlDialect dialect, DbConnection dbConnection, DbCommand dbCommand, Action<IReadOnlyCollection<DbParameter>?>? dbParametersCallback = null)
    {
        var mock = new Mock<IDbProvider<DbConnection>>();

        _ = mock.Setup(static db => db.GetDbConnection()).Returns(dbConnection);

        var m = mock.Setup(
            db => db.GetDbCommand(
                It.IsAny<DbConnection>(), It.IsAny<string>(), It.IsAny<IReadOnlyCollection<DbParameter>?>(), It.IsAny<int?>()))
            .Returns(dbCommand);

        if (dbParametersCallback is not null)
        {
            _ = m.Callback<DbConnection, string, IReadOnlyCollection<DbParameter>?, int?>(
                (_, _, actual, _) => dbParametersCallback.Invoke(actual));
        }

        _ = mock.SetupGet(static db => db.Dialect).Returns(dialect);

        return mock;
    }
}
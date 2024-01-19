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
            query: "Some SQL",
            parameters: new DbParameter[]
            {
                new("First", 3781.5m),
                new("Second", "")
            });

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
}
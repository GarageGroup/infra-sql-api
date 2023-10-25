using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbInsertQueryTest
{
    public static IEnumerable<object[]> SqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbInsertQuery(
                    tableName: "Country",
                    fieldValues: default),
                string.Empty
            },
            new object[]
            {
                new DbInsertQuery(
                    tableName: "SomeTable",
                    fieldValues: new DbFieldValue[]
                    {
                        new DbFieldValue("Id", 15)
                    }),
                "INSERT INTO SomeTable (Id) VALUES (@Id);"
            },
            new object[]
            {
                new DbInsertQuery(
                    tableName: "Country",
                    fieldValues: new DbFieldValue[]
                    {
                        new("Name", "Some value"),
                        new("Id", null, "Id1")
                    }),
                "INSERT INTO Country (Name, Id) VALUES (@Name, @Id1);"
            }
        };

    public static IEnumerable<object[]> ParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbInsertQuery(
                    tableName: "Country",
                    fieldValues: default),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbInsertQuery(
                    tableName: "Country",
                    fieldValues: new DbFieldValue[]
                    {
                        new DbFieldValue("Id", 15)
                    }),
                new FlatArray<DbParameter>(
                    new DbParameter("Id", 15))
            },
            new object[]
            {
                new DbInsertQuery(
                    tableName: "Country",
                    fieldValues: new DbFieldValue[]
                    {
                        new("Name", "Some value"),
                        new("Id", null, "Id1")
                    }),
                new FlatArray<DbParameter>(
                    new("Name", "Some value"),
                    new("Id1", null))
            }
        };
}
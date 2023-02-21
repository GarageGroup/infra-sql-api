using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Sql.Api.Core.Test;

public static partial class DbQueryTest
{
    public static IEnumerable<object[]> GetSqlQueryTestData()
        =>
        new[]
        {
            new object[]
            {
                new DbQuery(null!),
                string.Empty
            },
            new object[]
            {
                new DbQuery(string.Empty),
                string.Empty
            },
            new object[]
            {
                new DbQuery("Some SQL Query"),
                "Some SQL Query"
            },
            new object[]
            {
                new DbQuery(
                    query: null!,
                    parameters: new DbParameter[]
                    {
                        new("P1", 15),
                        new("P2", "Some text")
                    }),
                string.Empty
            },
            new object[]
            {
                new DbQuery(
                    query: string.Empty,
                    parameters: new DbParameter[]
                    {
                        new("SomeName", null)
                    }),
                string.Empty
            },
            new object[]
            {
                new DbQuery(
                    query: "Some query",
                    parameters: new DbParameter[]
                    {
                        new("SomeName1", "One"),
                        new("SomeName2", null)
                    }),
                "Some query"
            }
        };

    public static IEnumerable<object[]> GetParametersTestData()
        =>
        new[]
        {
            new object[]
            {
                new DbQuery(null!),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbQuery(string.Empty),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbQuery("Some SQL Query"),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbQuery(
                    query: null!,
                    parameters: new DbParameter[]
                    {
                        new("P1", 15),
                        new("P2", "Some text")
                    }),
                new FlatArray<DbParameter>(
                    new("P1", 15),
                    new("P2", "Some text"))
            },
            new object[]
            {
                new DbQuery(
                    query: string.Empty,
                    parameters: new DbParameter[]
                    {
                        new("SomeName", null)
                    }),
                new FlatArray<DbParameter>(
                    new DbParameter("SomeName", null))
            },
            new object[]
            {
                new DbQuery(
                    query: "Some query",
                    parameters: new DbParameter[]
                    {
                        new("SomeName1", "One"),
                        new("SomeName2", null)
                    }),
                new FlatArray<DbParameter>(
                    new("SomeName1", "One"),
                    new("SomeName2", null))
            }
        };
}
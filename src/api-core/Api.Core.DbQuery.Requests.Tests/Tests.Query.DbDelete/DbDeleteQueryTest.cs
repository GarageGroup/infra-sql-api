using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

public static partial class DbDeleteQueryTest
{
    public static IEnumerable<object[]> GetSqlQueryTestData()
        =>
        new[]
        {
            new object[]
            {
                new DbDeleteQuery(
                    tableName: "Country",
                    filter: new StubDbFilter("Id = 15")),
                "DELETE FROM Country WHERE Id = 15;"
            }
        };

    public static IEnumerable<object[]> GetParametersTestData()
        =>
        new[]
        {
            new object[]
            {
                new DbDeleteQuery(
                    tableName: "Country",
                    filter: new StubDbFilter("Id = 15")),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbDeleteQuery(
                    tableName: "Country",
                    filter: new StubDbFilter("Price > 0", new("Price", 25), new("Id", null))),
                new FlatArray<DbParameter>(new("Price", 25), new("Id", null))
            }
        };
}
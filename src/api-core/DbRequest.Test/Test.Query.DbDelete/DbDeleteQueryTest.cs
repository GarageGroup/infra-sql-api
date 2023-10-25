using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbDeleteQueryTest
{
    public static IEnumerable<object[]> SqlQueryTestData
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

    public static IEnumerable<object[]> ParametersTestData
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
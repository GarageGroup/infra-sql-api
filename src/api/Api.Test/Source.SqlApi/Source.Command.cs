using System.Collections.Generic;
using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class SqlApiTestSource
{
    public static TheoryData<StubDbQuery, SqlDialect, StubDbCommandRequest> DbCommandTestData
        =>
        new()
        {
            {
                new(
                    queries: new Dictionary<SqlDialect, string>
                    {
                        [SqlDialect.TransactSql] = string.Empty
                    },
                    parameters:
                    [
                        new("FirstParam", 71),
                        new("SecondParam", TestData.PlusFifteenIdRefType),
                        new("FirstParam", false),
                        new("FifthName", null)
                    ])
                {
                    TimeoutInSeconds = 51
                },
                SqlDialect.TransactSql,
                new()
                {
                    CommandText = string.Empty,
                    Parameters =
                    [
                        new("FirstParam", 71),
                        new("SecondParam", TestData.PlusFifteenIdRefType),
                        new("FifthName", null)
                    ],
                    Timeout = 51
                }
            },
            {
                new(
                    queries: new Dictionary<SqlDialect, string>
                    {
                        [SqlDialect.PostgreSql] = "SELECT * From Product"
                    },
                    parameters: default)
                {
                    TimeoutInSeconds = null
                },
                SqlDialect.PostgreSql,
                new()
                {
                    CommandText = "SELECT * From Product",
                    Parameters = [],
                    Timeout = null
                }
            }
        };
}
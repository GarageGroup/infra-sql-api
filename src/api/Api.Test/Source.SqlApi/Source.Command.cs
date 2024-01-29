using PrimeFuncPack.UnitTest;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

partial class SqlApiTestSource
{
    public static TheoryData<StubDbQuery, StubDbCommandRequest> DbCommandTestData
        =>
        new()
        {
            {
                new(
                    query: string.Empty,
                    parameters: new DbParameter[]
                    {
                        new("FirstParam", 71),
                        new("SecondParam", TestData.PlusFifteenIdRefType),
                        new("FirstParam", false),
                        new("FifthName", null)
                    })
                {
                    TimeoutInSeconds = 51
                },
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
                    query: "SELECT * From Product",
                    parameters: default)
                {
                    TimeoutInSeconds = null
                },
                new()
                {
                    CommandText = "SELECT * From Product",
                    Parameters = [],
                    Timeout = null
                }
            }
        };
}
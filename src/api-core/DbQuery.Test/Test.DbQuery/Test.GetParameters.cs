using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbQueryTest
{
    [Theory]
    [MemberData(nameof(ParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbQuery source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetParameters();
        Assert.StrictEqual(expected, actual);
    }

    public static TheoryData<DbQuery, FlatArray<DbParameter>> ParametersTestData
        =>
        new()
        {
            {
                new(null!),
                default
            },
            {
                new(string.Empty),
                default
            },
            {
                new("Some SQL Query"),
                default
            },
            {
                new(
                    query: null!,
                    parameters:
                    [
                        new("P1", 15),
                        new("P2", "Some text")
                    ]),
                [
                    new("P1", 15),
                    new("P2", "Some text")
                ]
            },
            {
                new(
                    query: string.Empty,
                    parameters:
                    [
                        new("SomeName", null)
                    ]),
                [
                    new("SomeName", null)
                ]
            },
            {
                new(
                    query: "Some query",
                    new("SomeName1", "One"),
                    new("SomeName2", null)),
                [
                    new("SomeName1", "One"),
                    new("SomeName2", null)
                ]
            }
        };
}
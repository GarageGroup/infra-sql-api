using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbQueryTest
{
    [Theory]
    [MemberData(nameof(SqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }

    public static TheoryData<DbQuery, string> SqlQueryTestData
        =>
        new()
        {
            {
                new(null!),
                string.Empty
            },
            {
                new(string.Empty),
                string.Empty
            },
            {
                new("Some SQL Query"),
                "Some SQL Query"
            },
            {
                new(
                    query: null!,
                    parameters: new DbParameter[]
                    {
                        new("P1", 15),
                        new("P2", "Some text")
                    }),
                string.Empty
            },
            {
                new(
                    query: string.Empty,
                    parameters: new DbParameter[]
                    {
                        new("SomeName", null)
                    }),
                string.Empty
            },
            {
                new(
                    query: "Some query",
                    parameters: new DbParameter[]
                    {
                        new("SomeName1", "One"),
                        new("SomeName2", null)
                    }),
                "Some query"
            }
        };
}
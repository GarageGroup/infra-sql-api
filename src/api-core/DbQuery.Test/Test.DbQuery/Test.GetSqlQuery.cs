using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbQueryTest
{
    [Theory]
    [MemberData(nameof(SqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(
        DbQuery source, SqlDialect dialect, string expected)
    {
        var actual = source.GetSqlQuery(dialect);
        Assert.Equal(expected, actual);
    }

    public static TheoryData<DbQuery, SqlDialect, string> SqlQueryTestData
        =>
        new()
        {
            {
                new(null!),
                SqlDialect.TransactSql,
                string.Empty
            },
            {
                new(string.Empty),
                SqlDialect.PostgreSql,
                string.Empty
            },
            {
                new("Some SQL Query"),
                SqlDialect.TransactSql,
                "Some SQL Query"
            },
            {
                new(
                    query: null!,
                    parameters:
                    [
                        new("P1", 15),
                        new("P2", "Some text")
                    ]),
                SqlDialect.TransactSql,
                string.Empty
            },
            {
                new(
                    query: string.Empty,
                    parameters:
                    [
                        new("SomeName", null)
                    ]),
                (SqlDialect)37,
                string.Empty
            },
            {
                new(
                    query: "Some query",
                    parameters:
                    [
                        new("SomeName1", "One"),
                        new("SomeName2", null)
                    ]),
                SqlDialect.PostgreSql,
                "Some query"
            }
        };
}
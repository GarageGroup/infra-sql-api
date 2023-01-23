using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbDeleteQueryTest
{
    [Theory]
    [MemberData(nameof(GetSqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbDeleteQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }
}
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbIfQueryTest
{
    [Theory]
    [MemberData(nameof(GetSqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbIfQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }
}
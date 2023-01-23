using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbCombinedFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_ExpectCorrectQuery(DbCombinedFilter source, string expected)
    {
        var actual = source.GetFilterSqlQuery();
        Assert.Equal(expected, actual);
    }
}
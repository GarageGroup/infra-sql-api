using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbCombinedFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_ExpectCorrectQuery(DbCombinedFilter filter, string expected)
    {
        var source = (IDbFilter)filter;
        var actual = source.GetFilterSqlQuery();

        Assert.Equal(expected, actual);
    }
}
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbLikeFilterTest
{
    [Theory]
    [MemberData(nameof(FilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_ExpectCorrectQuery(DbLikeFilter source, string expected)
    {
        var actual = source.GetFilterSqlQuery();
        Assert.Equal(expected, actual);
    }
}
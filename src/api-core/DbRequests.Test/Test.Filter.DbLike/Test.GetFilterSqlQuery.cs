using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbLikeFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_ExpectCorrectQuery(DbLikeFilter source, string expected)
    {
        var actual = source.GetFilterSqlQuery();
        Assert.Equal(expected, actual);
    }
}
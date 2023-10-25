using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbCombinedQueryTest
{
    [Theory]
    [MemberData(nameof(SqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbCombinedQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }
}
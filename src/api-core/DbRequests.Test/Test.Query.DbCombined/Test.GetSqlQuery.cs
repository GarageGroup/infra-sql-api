using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbCombinedQueryTest
{
    [Theory]
    [MemberData(nameof(GetSqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbCombinedQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }
}
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbQueryTest
{
    [Theory]
    [MemberData(nameof(GetSqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }
}
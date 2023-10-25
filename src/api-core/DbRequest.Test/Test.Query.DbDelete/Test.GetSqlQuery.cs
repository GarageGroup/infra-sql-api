using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbDeleteQueryTest
{
    [Theory]
    [MemberData(nameof(SqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbDeleteQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }
}
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbIfQueryTest
{
    [Theory]
    [MemberData(nameof(SqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbIfQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }
}
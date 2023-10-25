using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbUpdateQueryTest
{
    [Theory]
    [MemberData(nameof(SqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbUpdateQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }
}
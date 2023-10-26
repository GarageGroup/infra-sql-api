using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbRawFilterTest
{
    [Theory]
    [MemberData(nameof(FilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_ExpectCorrectSqlQuery(DbRawFilter source, string expected)
    {
        var actual = source.GetFilterSqlQuery();
        Assert.Equal(expected, actual);
    }
}
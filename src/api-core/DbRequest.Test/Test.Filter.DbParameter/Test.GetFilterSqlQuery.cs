using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbParameterFilterTest
{
    [Theory]
    [MemberData(nameof(FilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_OperatorIsInRange_ExpectCorrectSqlQuery(DbParameterFilter source, string expected)
    {
        var actual = source.GetFilterSqlQuery();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void GetFilterSqlQuery_OperatorIsOutOfRange_ExpectArgumentOutOfRangeException()
    {
        const int @operator = -1;
        var source = new DbParameterFilter("Id", (DbFilterOperator)@operator, 15);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(Test);
        Assert.Contains(@operator.ToString(), ex.Message, StringComparison.InvariantCulture);

        void Test()
            =>
            source.GetFilterSqlQuery();
    }
}
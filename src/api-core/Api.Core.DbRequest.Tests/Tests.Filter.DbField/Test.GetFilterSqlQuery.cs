using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbFieldFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_OperatorIsInRange_ExpectCorrectSqlQuery(DbFieldFilter source, string expected)
    {
        var filter = (IDbFilter)source;
        var actual = filter.GetFilterSqlQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void GetFilterSqlQuery_OperatorIsOutOfRange_ExpectArgumentOutOfRangeException()
    {
        const int @operator = -7;
        IDbFilter filter = new DbFieldFilter("Value", (DbFilterOperator)@operator, "25");

        var ex = Assert.Throws<ArgumentOutOfRangeException>(Test);
        Assert.Contains(@operator.ToString(), ex.Message, StringComparison.InvariantCulture);

        void Test()
            =>
            filter.GetFilterSqlQuery();
    }
}
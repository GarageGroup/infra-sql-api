using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbParameterFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterSqlQueryTestData))]
    public static void GetFilterSqlQuery_OperatorIsInRange_ExpectCorrectSqlQuery(DbParameterFilter source, string expected)
    {
        var filter = (IDbFilter)source;
        var actual = filter.GetFilterSqlQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void GetFilterSqlQuery_OperatorIsOutOfRange_ExpectArgumentOutOfRangeException()
    {
        const int @operator = -1;
        IDbFilter filter = new DbParameterFilter("Id", (DbFilterOperator)@operator, 15);

        var ex = Assert.Throws<ArgumentOutOfRangeException>(Test);
        Assert.Contains(@operator.ToString(), ex.Message, StringComparison.InvariantCulture);

        void Test()
            =>
            filter.GetFilterSqlQuery();
    }
}
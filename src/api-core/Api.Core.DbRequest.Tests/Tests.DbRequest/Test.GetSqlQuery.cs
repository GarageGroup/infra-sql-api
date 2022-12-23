using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbRequestTest
{
    [Theory]
    [MemberData(nameof(GetSqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbRequest source, string expected)
    {
        var query = (IDbQuery)source;
        var actual = query.GetSqlQuery();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void GetSqlQuery_JoinTypeIsOutOfRange_ExpectArgumentOutOfRangeException()
    {
        const int joinType = -5;

        IDbQuery query = new DbRequest("City")
        {
            JoinedTables = new FlatArray<DbJoinedTable>(
                new DbJoinedTable((DbJoinType)joinType, "Country", "c", new StubDbFilter("c.Id = 1")))
        };

        var ex = Assert.Throws<ArgumentOutOfRangeException>(Test);
        Assert.Contains(joinType.ToString(), ex.Message, StringComparison.InvariantCulture);

        void Test()
            =>
            query.GetSqlQuery();
    }

    [Fact]
    public static void GetSqlQuery_OrderTypeIsOutOfRange_ExpectArgumentOutOfRangeException()
    {
        const int orderType = -3;

        IDbQuery query = new DbRequest("Country")
        {
            Orders = new FlatArray<DbOrder>(
                new DbOrder("Name", (DbOrderType)orderType))
        };

        var ex = Assert.Throws<ArgumentOutOfRangeException>(Test);
        Assert.Contains(orderType.ToString(), ex.Message, StringComparison.InvariantCulture);

        void Test()
            =>
            query.GetSqlQuery();
    }
}
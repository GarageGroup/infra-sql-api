using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbSelectQueryTest
{
    [Theory]
    [MemberData(nameof(SqlQueryTestData))]
    public static void GetFilterSqlQuery_TypesAreInRange_ExpectCorrectSqlQuery(DbSelectQuery source, string expected)
    {
        var actual = source.GetSqlQuery();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public static void GetSqlQuery_JoinTypeIsOutOfRange_ExpectArgumentOutOfRangeException()
    {
        const int joinType = -5;

        var source = new DbSelectQuery("City")
        {
            JoinedTables = new FlatArray<DbJoinedTable>(
                new DbJoinedTable((DbJoinType)joinType, "Country", "c", new StubDbFilter("c.Id = 1")))
        };

        var ex = Assert.Throws<ArgumentOutOfRangeException>(Test);
        Assert.Contains(joinType.ToString(), ex.Message, StringComparison.InvariantCulture);

        void Test()
            =>
            source.GetSqlQuery();
    }

    [Fact]
    public static void GetSqlQuery_OrderTypeIsOutOfRange_ExpectArgumentOutOfRangeException()
    {
        const int orderType = -3;

        var source = new DbSelectQuery("Country")
        {
            Orders = new FlatArray<DbOrder>(
                new DbOrder("Name", (DbOrderType)orderType))
        };

        var ex = Assert.Throws<ArgumentOutOfRangeException>(Test);
        Assert.Contains(orderType.ToString(), ex.Message, StringComparison.InvariantCulture);

        void Test()
            =>
            source.GetSqlQuery();
    }
}
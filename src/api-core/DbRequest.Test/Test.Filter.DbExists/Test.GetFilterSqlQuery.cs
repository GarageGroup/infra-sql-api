using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbExistsFilterTest
{
    [Fact]
    public static void GetFilterSqlQuery_ExpectCorrectQuery()
    {
        var selectQuery = new DbSelectQuery("SomeTable")
        {
            SelectedFields = new("Id"),
            Filter = new StubDbFilter("Price > 0", new DbParameter("Price", 15))
        };
        var source = new DbExistsFilter(selectQuery);

        var expected = $"EXISTS ({selectQuery.GetSqlQuery()})";
        var actual = source.GetFilterSqlQuery();

        Assert.Equal(expected, actual);
    }
}
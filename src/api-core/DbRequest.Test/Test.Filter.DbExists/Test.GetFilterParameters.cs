using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbExistsFilterTest
{
    [Fact]
    public static void GetFilterParameters_ExpectCorrectParameters()
    {
        var selectQuery = new DbSelectQuery("SomeTable")
        {
            SelectedFields = new("Id"),
            Filter = new StubDbFilter("Price > 0", new("Price", 15), new("Name", "Some name"))
        };
        var source = new DbExistsFilter(selectQuery);

        var expected = selectQuery.GetParameters();
        var actual = source.GetFilterParameters();

        Assert.StrictEqual(expected, actual);
    }
}
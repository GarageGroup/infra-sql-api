using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbCombinedFilterTest
{
    [Theory]
    [MemberData(nameof(FilterParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbCombinedFilter source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetFilterParameters();
        Assert.Equal(expected, actual);
    }
}
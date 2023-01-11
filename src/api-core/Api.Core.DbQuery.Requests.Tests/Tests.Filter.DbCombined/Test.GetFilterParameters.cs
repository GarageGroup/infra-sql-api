using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbCombinedFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbCombinedFilter filter, FlatArray<DbParameter> expected)
    {
        var source = (IDbFilter)filter;
        var actual = source.GetFilterParameters();

        Assert.Equal(expected, actual);
    }
}
using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbParameterArrayFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbParameterArrayFilter source, FlatArray<DbParameter> expected)
    {
        var filter = (IDbFilter)source;
        var actual = filter.GetFilterParameters();

        Assert.StrictEqual(expected, actual);
    }
}
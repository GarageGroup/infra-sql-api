using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbParameterArrayFilterTest
{
    [Theory]
    [MemberData(nameof(FilterParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbParameterArrayFilter source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetFilterParameters();
        Assert.StrictEqual(expected, actual);
    }
}
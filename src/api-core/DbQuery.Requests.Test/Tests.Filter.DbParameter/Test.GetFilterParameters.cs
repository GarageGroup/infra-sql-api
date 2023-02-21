using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbParameterFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbParameterFilter source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetFilterParameters();
        Assert.StrictEqual(expected, actual);
    }
}
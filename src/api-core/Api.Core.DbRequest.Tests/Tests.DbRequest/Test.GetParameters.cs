using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbRequestTest
{
    [Theory]
    [MemberData(nameof(GetParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbRequest source, FlatArray<DbParameter> expected)
    {
        var query = (IDbQuery)source;
        var actual = query.GetParameters();

        Assert.StrictEqual(expected, actual);
    }
}
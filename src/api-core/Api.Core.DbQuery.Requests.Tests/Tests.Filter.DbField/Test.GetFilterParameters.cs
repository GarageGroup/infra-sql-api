using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbFieldFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterParametersTestData))]
    public static void GetFilterParameters_ExpectDefault(DbFieldFilter source)
    {
        var actual = source.GetFilterParameters();
        var expected = default(FlatArray<DbParameter>);

        Assert.StrictEqual(expected, actual);
    }
}
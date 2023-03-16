using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Test;

partial class DbRawFilterTest
{
    [Theory]
    [MemberData(nameof(GetFilterParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbRawFilter source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetFilterParameters();
        Assert.StrictEqual(expected, actual);
    }
}
using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbQueryTest
{
    [Theory]
    [MemberData(nameof(GetParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbQuery source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetParameters();
        Assert.StrictEqual(expected, actual);
    }
}
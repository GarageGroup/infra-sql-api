using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbDeleteQueryTest
{
    [Theory]
    [MemberData(nameof(ParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbDeleteQuery source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetParameters();
        Assert.StrictEqual(expected, actual);
    }
}
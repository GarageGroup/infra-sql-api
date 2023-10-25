using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbLikeFilterTest
{
    [Theory]
    [MemberData(nameof(FilterParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbLikeFilter source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetFilterParameters();
        Assert.Equal(expected, actual);
    }
}
using System;
using Xunit;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

partial class DbUpdateQueryTest
{
    [Theory]
    [MemberData(nameof(GetParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbUpdateQuery source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetParameters();
        Assert.StrictEqual(expected, actual);
    }
}
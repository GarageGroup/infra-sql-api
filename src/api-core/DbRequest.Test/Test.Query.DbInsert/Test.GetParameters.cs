using System;
using Xunit;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

partial class DbInsertQueryTest
{
    [Theory]
    [MemberData(nameof(ParametersTestData))]
    public static void GetFilterParameters_ExpectCorrectParameters(DbInsertQuery source, FlatArray<DbParameter> expected)
    {
        var actual = source.GetParameters();
        Assert.StrictEqual(expected, actual);
    }
}
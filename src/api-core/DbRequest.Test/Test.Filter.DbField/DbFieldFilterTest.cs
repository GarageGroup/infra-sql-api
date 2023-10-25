using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbFieldFilterTest
{
    public static IEnumerable<object[]> FilterSqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbFieldFilter("Id", DbFilterOperator.Equal, "1"),
                "Id = 1"
            },
            new object[]
            {
                new DbFieldFilter("Id", DbFilterOperator.Greater, "\"Some value\""),
                "Id > \"Some value\""
            },
            new object[]
            {
                new DbFieldFilter("p.id", DbFilterOperator.GreaterOrEqual, "75.34"),
                "p.id >= 75.34"
            },
            new object[]
            {
                new DbFieldFilter("Name", DbFilterOperator.GreaterOrEqual, string.Empty),
                "Name >= null"
            },
            new object[]
            {
                new DbFieldFilter("Id", DbFilterOperator.Less, null!),
                "Id < null"
            },
            new object[]
            {
                new DbFieldFilter("value", DbFilterOperator.LessOrEqual, "(SELECT COUNT(*) FROM Country)"),
                "value <= (SELECT COUNT(*) FROM Country)"
            },
            new object[]
            {
                new DbFieldFilter("Id", DbFilterOperator.Inequal, "\t"),
                "Id <> null"
            }
        };

    public static IEnumerable<object[]> FilterParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbFieldFilter("Id", DbFilterOperator.Equal, "73")
            },
            new object[]
            {
                new DbFieldFilter("Id", DbFilterOperator.Greater, string.Empty)
            },
            new object[]
            {
                new DbFieldFilter("Value", DbFilterOperator.Equal, "(SELECT COUNT(*) FROM Country)")
            },
            new object[]
            {
                new DbFieldFilter("Name", DbFilterOperator.Inequal, "true")
            },
            new object[]
            {
                new DbFieldFilter("value", (DbFilterOperator)(-3), "\"Some text\"")
            }
        };
}
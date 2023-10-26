using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbParameterArrayFilterTest
{
    public static IEnumerable<object[]> FilterSqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbParameterArrayFilter("Id", DbArrayFilterOperator.In, default, "IdParam"),
                string.Empty
            },
            new object[]
            {
                new DbParameterArrayFilter("Id", DbArrayFilterOperator.In, new(1, 2, 3), "IdParam"),
                "Id IN (@IdParam0, @IdParam1, @IdParam2)"
            },
            new object[]
            {
                new DbParameterArrayFilter("Value", DbArrayFilterOperator.In, new("Some text", null)),
                "Value IN (@Value0, @Value1)"
            },
            new object[]
            {
                new DbParameterArrayFilter("Price", DbArrayFilterOperator.In, new(string.Empty), string.Empty),
                "Price IN (@Price0)"
            },
            new object[]
            {
                new DbParameterArrayFilter("Id", DbArrayFilterOperator.NotIn, default),
                string.Empty
            },
            new object[]
            {
                new DbParameterArrayFilter("Id", DbArrayFilterOperator.NotIn, new(1, 2)),
                "Id NOT IN (@Id0, @Id1)"
            },
            new object[]
            {
                new DbParameterArrayFilter("Price", DbArrayFilterOperator.NotIn, new(null, 1, 15), string.Empty),
                "Price NOT IN (@Price0, @Price1, @Price2)"
            },
            new object[]
            {
                new DbParameterArrayFilter("Id", DbArrayFilterOperator.NotIn, new("Some value"), "IdParam"),
                "Id NOT IN (@IdParam0)"
            }
        };

    public static IEnumerable<object[]> FilterParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbParameterArrayFilter("Id", DbArrayFilterOperator.In, default),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbParameterArrayFilter("Id", DbArrayFilterOperator.In, new(1, 2, null), "IdParam"),
                new FlatArray<DbParameter>(
                    new("IdParam0", 1),
                    new("IdParam1", 2),
                    new("IdParam2", null))
            },
            new object[]
            {
                new DbParameterArrayFilter("Price", DbArrayFilterOperator.NotIn, default, "PriceParam"),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbParameterArrayFilter("Value", (DbArrayFilterOperator)(-1), new(-7, null, "Some text"), string.Empty),
                new FlatArray<DbParameter>(
                    new("Value0", -7),
                    new("Value1", null),
                    new("Value2", "Some text"))
            }
        };
}
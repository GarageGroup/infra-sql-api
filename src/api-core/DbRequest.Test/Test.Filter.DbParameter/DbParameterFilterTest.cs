using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbParameterFilterTest
{
    public static IEnumerable<object[]> FilterSqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbParameterFilter("Id", DbFilterOperator.Equal, 1),
                "Id = @Id"
            },
            new object[]
            {
                new DbParameterFilter("Id", DbFilterOperator.Equal, null),
                "Id = @Id"
            },
            new object[]
            {
                new DbParameterFilter("a.id", DbFilterOperator.Equal, "Some value", "p.Id"),
                "a.id = @p.Id"
            },
            new object[]
            {
                new DbParameterFilter("Price", DbFilterOperator.Greater, 104.51m, "Value"),
                "Price > @Value"
            },
            new object[]
            {
                new DbParameterFilter("Name", DbFilterOperator.GreaterOrEqual, "Some string", string.Empty),
                "Name >= @Name"
            },
            new object[]
            {
                new DbParameterFilter("Id", DbFilterOperator.Less, 11),
                "Id < @Id"
            },
            new object[]
            {
                new DbParameterFilter("value", DbFilterOperator.LessOrEqual, null),
                "value <= @value"
            },
            new object[]
            {
                new DbParameterFilter("Id", DbFilterOperator.Inequal, 1),
                "Id <> @Id"
            }
        };

    public static IEnumerable<object[]> FilterParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbParameterFilter("Id", DbFilterOperator.Equal, 1),
                new FlatArray<DbParameter>(
                    new DbParameter("Id", 1))
            },
            new object[]
            {
                new DbParameterFilter("Id", DbFilterOperator.Greater, null),
                new FlatArray<DbParameter>(
                    new DbParameter("Id", null))
            },
            new object[]
            {
                new DbParameterFilter("id", DbFilterOperator.Equal, "Some value", "p.Id"),
                new FlatArray<DbParameter>(
                    new DbParameter("p.Id", "Some value"))
            },
            new object[]
            {
                new DbParameterFilter("Name", DbFilterOperator.GreaterOrEqual, false, string.Empty),
                new FlatArray<DbParameter>(
                    new DbParameter("Name", false))
            },
            new object[]
            {
                new DbParameterFilter("value", (DbFilterOperator)(-1), 75.91m),
                new FlatArray<DbParameter>(
                    new DbParameter("value", 75.91m))
            }
        };
}
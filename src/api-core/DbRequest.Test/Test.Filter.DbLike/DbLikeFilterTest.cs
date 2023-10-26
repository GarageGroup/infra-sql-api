using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbLikeFilterTest
{
    public static IEnumerable<object[]> FilterSqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbLikeFilter("LOWER(p.Name)", "TeSt", "Search"),
                "LOWER(p.Name) LIKE '%' + @Search + '%'"
            },
            new object[]
            {
                new DbLikeFilter("Field1", null, "Field1"),
                "Field1 LIKE '%' + @Field1 + '%'"
            },
            new object[]
            {
                new DbLikeFilter("p.Name", "\t", "Name"),
                "p.Name LIKE '%' + @Name + '%'"
            }
        };

    public static IEnumerable<object[]> FilterParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbLikeFilter("LOWER(p.Name)", "TeSt", "Search"),
                new FlatArray<DbParameter>(
                    new DbParameter("Search", "TeSt"))
            },
            new object[]
            {
                new DbLikeFilter("p.Name", null, "Name"),
                new FlatArray<DbParameter>(
                    new DbParameter("Name", null))
            },
            new object[]
            {
                new DbLikeFilter("Description", "\t\n", "DescriptionParameter"),
                new FlatArray<DbParameter>(
                    new DbParameter("DescriptionParameter", "\t\n"))
            }
        };
}
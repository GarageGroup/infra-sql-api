using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbCombinedFilterTest
{
    public static IEnumerable<object[]> FilterSqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbCombinedFilter(DbLogicalOperator.And, default),
                string.Empty
            },
            new object[]
            {
                new DbCombinedFilter(DbLogicalOperator.Or, default),
                string.Empty
            },
            new object[]
            {
                new DbCombinedFilter(
                    DbLogicalOperator.And,
                    new StubDbFilter[]
                    {
                        new(
                            string.Empty,
                            new DbParameter[]
                            {
                                new("Id", 151)
                            })
                    }),
                string.Empty
            },
            new object[]
            {
                new DbCombinedFilter(DbLogicalOperator.Or)
                {
                    Filters = new StubDbFilter[]
                    {
                        new(
                            string.Empty,
                            new DbParameter[]
                            {
                                new("Name", "Some string")
                            })
                    }
                },
                string.Empty
            },
            new object[]
            {
                new DbCombinedFilter(
                    DbLogicalOperator.And,
                    new StubDbFilter[]
                    {
                        new(
                            "Name = @Name",
                            new DbParameter[]
                            {
                                new("Name", "Some string")
                            })
                    }),
                "Name = @Name"
            },
            new object[]
            {
                new DbCombinedFilter(
                    DbLogicalOperator.Or,
                    new StubDbFilter[]
                    {
                        new(
                            "Id = @Id",
                            new DbParameter[]
                            {
                                new("Id", 21)
                            })
                    }),
                "Id = @Id"
            },
            new object[]
            {
                new DbCombinedFilter(DbLogicalOperator.And)
                {
                    Filters = new StubDbFilter[]
                    {
                        new(
                            "Name <> @Name",
                            new DbParameter[]
                            {
                                new("Name", "Some name")
                            }),
                        new(
                            "Id = @Id OR Sum = NULL",
                            new DbParameter[]
                            {
                                new("Param1", null),
                                new("Param2", 57),
                                new("Param3", 105)
                            }),
                        new(
                            string.Empty,
                            new DbParameter[]
                            {
                                new("Id", 275)
                            }),
                    }
                },
                "(Name <> @Name AND Id = @Id OR Sum = NULL)"
            },
            new object[]
            {
                new DbCombinedFilter(
                    DbLogicalOperator.Or,
                    new StubDbFilter[]
                    {
                        new("Id > 1"),
                        new(
                            "\t",
                            new DbParameter[]
                            {
                                new("Name", "Some string")
                            }),
                        new(
                            "Id < 5",
                            new DbParameter[]
                            {
                                new("Param1", 15),
                                new("Param2", null),
                                new("Param3", -1)
                            })
                    }),
                "(Id > 1 OR Id < 5)"
            }
        };

    public static IEnumerable<object[]> FilterParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbCombinedFilter(DbLogicalOperator.And, default),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbCombinedFilter(DbLogicalOperator.Or, default),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbCombinedFilter(
                    DbLogicalOperator.Or,
                    new StubDbFilter[]
                    {
                        new("Id > 0")
                    }),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbCombinedFilter(
                    DbLogicalOperator.And,
                    new StubDbFilter[]
                    {
                        new(
                            "HasBalcony = @HasBalcony",
                            new DbParameter[]
                            {
                                new("HasBalcony", false)
                            })
                    }),
                new FlatArray<DbParameter>(
                    new DbParameter("HasBalcony", false))
            },
            new object[]
            {
                new DbCombinedFilter(
                    DbLogicalOperator.And,
                    new StubDbFilter[]
                    {
                        new(
                            "Id > @Id",
                            new DbParameter[]
                            {
                                new("Id", 15)
                            }),
                        new(
                            "Name = @Name",
                            new DbParameter[]
                            {
                                new("Name", null)
                            }),
                        new("Price > 0")
                    }),
                new FlatArray<DbParameter>(
                    new("Id", 15),
                    new("Name", null))
            },
            new object[]
            {
                new DbCombinedFilter(
                    DbLogicalOperator.Or,
                    new StubDbFilter[]
                    {
                        new(
                            "Id > @Id",
                            new DbParameter[]
                            {
                                new("Id", 15)
                            }),
                        new(
                            "\t",
                            new DbParameter[]
                            {
                                new("Name", "Some string")
                            }),
                        new(
                            "Price IN (@Price0, @Price1)",
                            new DbParameter[]
                            {
                                new("Price0", null),
                                new("Price1", 10.51m)
                            })
                    }),
                new FlatArray<DbParameter>(
                    new("Id", 15),
                    new("Name", "Some string"),
                    new("Price0", null),
                    new("Price1", 10.51m))
            }
        };
}
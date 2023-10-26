using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbUpdateQueryTest
{
    public static IEnumerable<object[]> SqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbUpdateQuery(
                    tableName: "Country",
                    fieldValues: default,
                    filter: new StubDbFilter("Price > 0")),
                string.Empty
            },
            new object[]
            {
                new DbUpdateQuery(
                    tableName: "SomeTable",
                    fieldValues: new DbFieldValue[]
                    {
                        new DbFieldValue("Id", 15)
                    },
                    filter: new StubDbFilter("Price > 0")),
                "UPDATE SomeTable SET Id = @Id WHERE Price > 0;"
            },
            new object[]
            {
                new DbUpdateQuery(
                    tableName: "Country",
                    fieldValues: new DbFieldValue[]
                    {
                        new("Name", "Some value"),
                        new("Id", null, "Id1")
                    },
                    filter: new StubDbFilter("Price > 0")),
                "UPDATE Country SET Name = @Name, Id = @Id1 WHERE Price > 0;"
            }
        };

    public static IEnumerable<object[]> ParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbUpdateQuery(
                    tableName: "Country",
                    fieldValues: default,
                    filter: new StubDbFilter("Price > 0", new DbParameter("SomeParam", 15))),
                new FlatArray<DbParameter>(
                    new DbParameter("SomeParam", 15))
            },
            new object[]
            {
                new DbUpdateQuery(
                    tableName: "Country",
                    fieldValues: new DbFieldValue[]
                    {
                        new DbFieldValue("Id", 15)
                    },
                    filter: new StubDbFilter("Price > 0")),
                new FlatArray<DbParameter>(
                    new DbParameter("Id", 15))
            },
            new object[]
            {
                new DbUpdateQuery(
                    tableName: "Country",
                    fieldValues: new DbFieldValue[]
                    {
                        new("Name", "Some value"),
                        new("Id", null, "Id1")
                    },
                    filter: new StubDbFilter("Price > 0", new DbParameter("Price", 25))),
                new FlatArray<DbParameter>(
                    new("Name", "Some value"),
                    new("Id1", null),
                    new("Price", 25))
            }
        };
}
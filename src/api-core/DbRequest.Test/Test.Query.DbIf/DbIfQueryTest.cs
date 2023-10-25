using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbIfQueryTest
{
    public static IEnumerable<object[]> SqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbIfQuery(
                    condition: new StubDbFilter("Id = @Id", new DbParameter("Id", 15)),
                    thenQuery: new StubDbQuery("SELECT * FROM Country")),
                "IF Id = @Id\n" +
                "BEGIN\n" +
                "SELECT * FROM Country\n" +
                "END"
            },
            new object[]
            {
                new DbIfQuery(
                    condition: new StubDbFilter("Id = @Id", new DbParameter("Id", null)),
                    thenQuery: new StubDbQuery("SELECT * FROM Country"),
                    elseQuery: new StubDbQuery("SELECT Price, Name FROM Product WHERE Price > @Price", new DbParameter("Price", 1000))),
                "IF Id = @Id\n" +
                "BEGIN\n" +
                "SELECT * FROM Country\n" +
                "END\n" +
                "ELSE\n" +
                "BEGIN\n" +
                "SELECT Price, Name FROM Product WHERE Price > @Price\n" +
                "END"
            }
        };

    public static IEnumerable<object[]> ParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbIfQuery(
                    condition: new StubDbFilter("Id = @Id"),
                    thenQuery: new StubDbQuery("SELECT * FROM Country")),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbIfQuery(
                    condition: new StubDbFilter("Id = @Id", new("Id", null), new("Name", "SomeName")),
                    thenQuery: new StubDbQuery("SELECT * FROM Country", new DbParameter("SomeParameter", 200)),
                    elseQuery: new StubDbQuery("SELECT Price, Name FROM Product WHERE Price > @Price", new DbParameter("Price", 1000))),
                new FlatArray<DbParameter>(
                    new("Id", null),
                    new("Name", "SomeName"),
                    new("SomeParameter", 200),
                    new("Price", 1000))
            }
        };
}
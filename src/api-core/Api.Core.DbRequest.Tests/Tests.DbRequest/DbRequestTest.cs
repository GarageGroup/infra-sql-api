using System;
using System.Collections.Generic;

namespace GGroupp.Infra.Sql.Api.Core.Tests;

public static partial class DbRequestTest
{
    public static IEnumerable<object[]> GetSqlQueryTestData()
        =>
        new[]
        {
            new object[]
            {
                new DbRequest("Country"),
                "SELECT * FROM Country"
            },
            new object[]
            {
                new DbRequest("Country")
                {
                    Top = 5
                },
                "SELECT TOP 5 * FROM Country"
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    Filters = new(
                        new StubDbFilter("HasBalcony = @HasBalcony", new DbParameter("HasBalcony", false)))
                },
                "SELECT * FROM Property p WHERE HasBalcony = @HasBalcony"
            },
            new object[]
            {
                new DbRequest("Property", "\t")
                {
                    SelectedFields = new("Id", "Name"),
                    Filters = new(
                        new StubDbFilter("Id > @Id", new DbParameter("Id", 15)),
                        new StubDbFilter("Name = @Name", new DbParameter("Name", null)),
                        new StubDbFilter("Price > 0"))
                },
                "SELECT Id, Name FROM Property WHERE (Id > @Id AND Name = @Name AND Price > 0)"
            },
            new object[]
            {
                new DbRequest("Property")
                {
                    SelectedFields = new("Id", "Name"),
                    Filters = new(
                        new StubDbFilter("Id > @Id", new DbParameter("Id", 15)),
                        new StubDbFilter("\t", new DbParameter("Name", "Some string")),
                        new StubDbFilter("Price IN (@Price0, @Price1)", new("Price0", null), new("Price1", 10.51m)))
                },
                "SELECT Id, Name FROM Property WHERE (Id > @Id AND Price IN (@Price0, @Price1))"
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    JoinedTables = new(
                        new DbJoinedTable(
                            DbJoinType.Inner, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id"), new StubDbFilter("t.Id > 0")))
                },
                "SELECT * FROM Property p INNER JOIN Translation t ON (t.PropertyId = p.Id AND t.Id > 0)"
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    Filters = new(
                        new StubDbFilter("p.Id > @Id", new DbParameter("Id", 15))),
                    JoinedTables = new(
                        new(DbJoinType.Left, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id")),
                        new(DbJoinType.Right, "City", "c", new StubDbFilter("c.Id <> p.CityId")))
                },
                "SELECT * FROM Property p LEFT JOIN Translation t ON t.PropertyId = p.Id RIGHT JOIN City c ON c.Id <> p.CityId WHERE p.Id > @Id"
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    SelectedFields = new("p.Id", "\t", "t.Id", null!),
                    Filters = new(
                        new StubDbFilter("p.Id > @Id", new DbParameter("Id", 15))),
                    JoinedTables = new(
                        new(DbJoinType.Left, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id")),
                        new(DbJoinType.Right, "City", "c", new StubDbFilter("c.Id <> p.CityId")))
                },
                "SELECT p.Id, t.Id FROM Property p LEFT JOIN Translation t ON t.PropertyId = p.Id RIGHT JOIN City c ON c.Id <> p.CityId" +
                " WHERE p.Id > @Id"
            },
            new object[]
            {
                new DbRequest("Property")
                {
                    Orders = new(
                        new DbOrder("CrmId", DbOrderType.Ascending))
                },
                "SELECT * FROM Property ORDER BY CrmId ASC"
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    Orders = new(
                        new("p.CrmId", DbOrderType.Descending),
                        new("Id", DbOrderType.Ascending),
                        new("Price"))
                },
                "SELECT * FROM Property p ORDER BY p.CrmId DESC, Id ASC, Price"
            },
            new object[]
            {
                new DbRequest("Property")
                {
                    Orders = new(new("Id"), new("Name", DbOrderType.Descending)),
                    Offset = 7
                },
                "SELECT * FROM Property ORDER BY Id, Name DESC OFFSET 7"
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    Top = 7,
                    Offset = 5071,
                    SelectedFields = new("p.Id", "t.Id"),
                    Filters = new(
                        new StubDbFilter("p.Id > @Id", new DbParameter("Id", 15))),
                    JoinedTables = new(
                        new(DbJoinType.Left, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id")),
                        new(DbJoinType.Right, "City", "c", new StubDbFilter("c.Id <> p.CityId"))),
                    Orders = new(
                        new("p.Id", DbOrderType.Descending),
                        new("c.Id"))
                },
                "SELECT p.Id, t.Id FROM Property p LEFT JOIN Translation t ON t.PropertyId = p.Id RIGHT JOIN City c ON c.Id <> p.CityId" +
                " WHERE p.Id > @Id ORDER BY p.Id DESC, c.Id OFFSET 5071 ROWS FETCH NEXT 7 ROWS ONLY"
            }
        };

    public static IEnumerable<object[]> GetParametersTestData()
        =>
        new[]
        {
            new object[]
            {
                new DbRequest("Country"),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbRequest("Country")
                {
                    Top = 5,
                    Offset = long.MaxValue,
                    SelectedFields = new("Id", "Name"),
                    Filters = new FlatArray<IDbFilter>(
                        new StubDbFilter("Id > 0")),
                    Orders = new FlatArray<DbOrder>(
                        new DbOrder("Name"))
                },
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    Filters = new(
                        new StubDbFilter("HasBalcony = @HasBalcony", new DbParameter("HasBalcony", false)))
                },
                new FlatArray<DbParameter>(
                    new DbParameter("HasBalcony", false))
            },
            new object[]
            {
                new DbRequest("Property", "\t")
                {
                    SelectedFields = new("Id", "Name"),
                    Filters = new(
                        new StubDbFilter("Id > @Id", new DbParameter("Id", 15)),
                        new StubDbFilter("Name = @Name", new DbParameter("Name", null)),
                        new StubDbFilter("Price > 0"))
                },
                new FlatArray<DbParameter>(
                    new("Id", 15), new("Name", null))
            },
            new object[]
            {
                new DbRequest("Property")
                {
                    SelectedFields = new("Id", "Name"),
                    Filters = new(
                        new StubDbFilter("Id > @Id", new DbParameter("Id", 15)),
                        new StubDbFilter("\t", new DbParameter("Name", "Some string")),
                        new StubDbFilter("Price IN (@Price0, @Price1)", new("Price0", null), new("Price1", 10.51m)))
                },
                new FlatArray<DbParameter>(
                    new("Id", 15), new("Name", "Some string"), new("Price0", null), new("Price1", 10.51m))
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    JoinedTables = new(
                        new DbJoinedTable(
                            DbJoinType.Inner, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id", new DbParameter("Id", 101))))
                },
                new FlatArray<DbParameter>(
                    new DbParameter("Id", 101))
            },
            new object[]
            {
                new DbRequest("Property", "p")
                {
                    Top = 15,
                    SelectedFields = new("p.Id", "t.Id"),
                    Filters = new(
                        new StubDbFilter("Id > @Id", new DbParameter("Id", 15)),
                        new StubDbFilter("\t", new DbParameter("Name", "Some string")),
                        new StubDbFilter("Price IN (@Price0, @Price1)", new("Price0", null), new("Price1", 10.51m))),
                    JoinedTables = new(
                        new(DbJoinType.Left, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id", new DbParameter("Value", null))),
                        new(DbJoinType.Right, "City", "c", new StubDbFilter("c.Id <> p.CityId", new DbParameter("CityValue", "Some value"))))
                },
                new FlatArray<DbParameter>(
                    new("Id", 15),
                    new("Name", "Some string"),
                    new("Price0", null),
                    new("Price1", 10.51m),
                    new("Value", null),
                    new("CityValue", "Some value"))
            }
        };
}
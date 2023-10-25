using System;
using System.Collections.Generic;

namespace GarageGroup.Infra.Sql.Api.Core.Test;

public static partial class DbSelectQueryTest
{
    public static IEnumerable<object[]> SqlQueryTestData
        =>
        new[]
        {
            new object[]
            {
                new DbSelectQuery("Country"),
                "SELECT * FROM Country"
            },
            new object[]
            {
                new DbSelectQuery("Country")
                {
                    Top = 5
                },
                "SELECT TOP 5 * FROM Country"
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    Filter = new StubDbFilter("HasBalcony = 1")
                },
                "SELECT * FROM Property p WHERE HasBalcony = 1"
            },
            new object[]
            {
                new DbSelectQuery("Property", "\t")
                {
                    SelectedFields = new("Id", "SUM(Price) AS Price"),
                    Filter = new StubDbFilter(
                        "Id > @Id AND Name = @Name AND Price > 0",
                        new("HasBalcony", false),
                        new("Name", null)),
                    GroupByFields = new("Id")
                },
                "SELECT Id, SUM(Price) AS Price FROM Property WHERE Id > @Id AND Name = @Name AND Price > 0 GROUP BY Id"
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    JoinedTables = new(
                        new DbJoinedTable(
                            DbJoinType.Inner, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id")))
                },
                "SELECT * FROM Property p INNER JOIN Translation t ON t.PropertyId = p.Id"
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    Filter = new StubDbFilter("p.Id > @Id", new DbParameter("Id", 15)),
                    JoinedTables = new(
                        new(DbJoinType.Left, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id")),
                        new(DbJoinType.Right, "City", "c", new StubDbFilter("c.Id <> p.CityId")))
                },
                "SELECT * FROM Property p LEFT JOIN Translation t ON t.PropertyId = p.Id RIGHT JOIN City c ON c.Id <> p.CityId WHERE p.Id > @Id"
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    SelectedFields = new("p.Id", "\t", "t.Id", null!),
                    Filter = new StubDbFilter("p.Id > @Id", new DbParameter("Id", 15)),
                    JoinedTables = new(
                        new(DbJoinType.Left, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id")),
                        new(DbJoinType.Right, "City", "c", new StubDbFilter("c.Id <> p.CityId"))),
                    GroupByFields = new("p.Id", null!, "t.Id", "\n\r"),
                    Orders = new DbOrder("Date").AsFlatArray()
                },
                "SELECT p.Id, t.Id FROM Property p LEFT JOIN Translation t ON t.PropertyId = p.Id RIGHT JOIN City c ON c.Id <> p.CityId" +
                " WHERE p.Id > @Id GROUP BY p.Id, t.Id ORDER BY Date"
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    AppliedTables = new(
                        new DbAppliedTable(
                            DbApplyType.Outer, new("PropertyOwner", "po"), "po"),
                        new DbAppliedTable(
                            DbApplyType.Cross, new("PropertyType", "pt"), "pt"))
                },
                "SELECT * FROM Property p OUTER APPLY (SELECT * FROM PropertyOwner po) po " +
                "CROSS APPLY (SELECT * FROM PropertyType pt) pt"
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    Filter = new StubDbFilter("p.Id > @Id", new DbParameter("Id", 15)),
                    AppliedTables = new(
                        new DbAppliedTable(
                            DbApplyType.Cross, new("PropertyOwner", "po"), "po"))
                },
                "SELECT * FROM Property p CROSS APPLY (SELECT * FROM PropertyOwner po) po" +
                " WHERE p.Id > @Id"
            },
            new object[]
            {
                new DbSelectQuery("Property")
                {
                    Orders = new(
                        new DbOrder("CrmId", DbOrderType.Ascending))
                },
                "SELECT * FROM Property ORDER BY CrmId ASC"
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
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
                new DbSelectQuery("Property")
                {
                    Orders = new(new("Id"), new("Name", DbOrderType.Descending)),
                    Offset = 7
                },
                "SELECT * FROM Property ORDER BY Id, Name DESC OFFSET 7"
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    Top = 7,
                    Offset = 5071,
                    SelectedFields = new("p.Id", "t.Id"),
                    Filter = new StubDbFilter("p.Id > @Id", new DbParameter("Id", 15)),
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

    public static IEnumerable<object[]> ParametersTestData
        =>
        new[]
        {
            new object[]
            {
                new DbSelectQuery("Country"),
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbSelectQuery("Country")
                {
                    Top = 5,
                    Offset = long.MaxValue,
                    SelectedFields = new("Id", "Name"),
                    Filter = new StubDbFilter("Id > 0"),
                    Orders = new FlatArray<DbOrder>(
                        new DbOrder("Name"))
                },
                default(FlatArray<DbParameter>)
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    Filter = new StubDbFilter("HasBalcony = @HasBalcony", new DbParameter("HasBalcony", false))
                },
                new FlatArray<DbParameter>(
                    new DbParameter("HasBalcony", false))
            },
            new object[]
            {
                new DbSelectQuery("Property", "\t")
                {
                    SelectedFields = new("Id", "Name"),
                    Filter = new StubDbFilter("Id > @Id", new("Id", 15), new("Name", null)),
                    GroupByFields = new("Id", "Name")
                },
                new FlatArray<DbParameter>(
                    new("Id", 15), new("Name", null))
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
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
                new DbSelectQuery("Property", "p")
                {
                    Top = 15,
                    SelectedFields = new("p.Id", "t.Id"),
                    Filter = new StubDbFilter(
                        "Id > @Id", new("Id", 15), new("Name", "Some string"), new("Price0", null), new("Price1", 10.51m)),
                    JoinedTables = new(
                        new(DbJoinType.Left, "Translation", "t", new StubDbFilter("t.PropertyId = p.Id", new DbParameter("Value", null))),
                        new(DbJoinType.Right, "City", "c", new StubDbFilter("c.Id <> p.CityId", new DbParameter("CityValue", "Some value")))),
                    GroupByFields = new("p.Id")
                },
                new FlatArray<DbParameter>(
                    new("Id", 15),
                    new("Name", "Some string"),
                    new("Price0", null),
                    new("Price1", 10.51m),
                    new("Value", null),
                    new("CityValue", "Some value"))
            },
            new object[]
            {
                new DbSelectQuery("Property", "p")
                {
                    AppliedTables = new(
                        new DbAppliedTable(
                            type: DbApplyType.Cross,
                            selectQuery: new("PropertyType", "pt")
                            {
                                Filter = new StubDbFilter(
                                    "Name = @Name", new("Id", 5), new("Name", "Test"))
                            },
                            alias: "pt"))
                },
                new FlatArray<DbParameter>(
                    new DbParameter("Id", 5),
                    new DbParameter("Name", "Test"))
            },
        };
}
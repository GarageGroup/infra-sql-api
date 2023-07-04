using System;

namespace GarageGroup.Infra;

public sealed record class DbOrder
{
    public DbOrder(string fieldName, DbOrderType orderType = default)
    {
        FieldName = fieldName.OrEmpty();
        OrderType = orderType;
    }

    public string FieldName { get; }

    public DbOrderType OrderType { get; }
}
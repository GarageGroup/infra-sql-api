using System;

namespace GarageGroup.Infra;

public interface IDbQuery
{
    string GetSqlQuery();

    FlatArray<DbParameter> GetParameters();

    int? TimeoutInSeconds { get; }
}
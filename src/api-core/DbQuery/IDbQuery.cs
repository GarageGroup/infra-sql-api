using System;

namespace GarageGroup.Infra;

public interface IDbQuery
{
    string GetSqlQuery(SqlDialect dialect);

    FlatArray<DbParameter> GetParameters();

    int? TimeoutInSeconds { get; }
}
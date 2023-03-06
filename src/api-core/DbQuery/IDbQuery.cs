using System;

namespace GGroupp.Infra;

public interface IDbQuery
{
    string GetSqlQuery();

    FlatArray<DbParameter> GetParameters();

    int? TimeoutInSeconds { get; }
}
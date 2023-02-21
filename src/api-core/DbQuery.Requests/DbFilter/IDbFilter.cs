using System;

namespace GGroupp.Infra;

public interface IDbFilter
{
    string GetFilterSqlQuery();

    FlatArray<DbParameter> GetFilterParameters();
}
using System;

namespace GarageGroup.Infra;

public interface IDbFilter
{
    string GetFilterSqlQuery();

    FlatArray<DbParameter> GetFilterParameters();
}
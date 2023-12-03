using System.Data.Common;

namespace GarageGroup.Infra;

public interface IDbProvider
{
    DbConnection GetDbConnection();

    object GetSqlParameter(DbParameter parameter);
}
using System.Data.Common;

namespace GGroupp.Infra;

public interface IDbProvider
{
    DbConnection GetDbConnection();

    object GetSqlParameter(DbParameter parameter);
}
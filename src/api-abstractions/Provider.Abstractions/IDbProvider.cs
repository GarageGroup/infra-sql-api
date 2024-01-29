using System;
using System.Data.Common;

namespace GarageGroup.Infra;

[Obsolete("This type is obsolete. Use IDbProvider<TDbConnection> instead")]
public interface IDbProvider
{
    DbConnection GetDbConnection();

    object GetSqlParameter(DbParameter parameter);
}
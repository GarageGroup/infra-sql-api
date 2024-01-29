using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace GarageGroup.Infra;

public interface IDbProvider<TDbConnection>
    where TDbConnection : DbConnection
{
    TDbConnection GetDbConnection();

    DbCommand GetDbCommand(
        TDbConnection dbConnection,
        string commandText,
        [AllowNull] IReadOnlyCollection<DbParameter> parameters,
        int? timeoutInSeconds);
}
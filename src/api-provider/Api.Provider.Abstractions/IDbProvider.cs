using System.Collections.Generic;
using System.Data.Common;

namespace GGroupp.Infra;

public interface IDbProvider
{
    DbConnection GetDbConnection();

    object GetDbParameter(KeyValuePair<string, object?> parameter);
}
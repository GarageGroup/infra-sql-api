using System.Data;
using System.Data.Common;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

internal interface IStubDbCommand
{
    int ExecuteNonQuery();

    DbDataReader ExecuteDbDataReader(CommandBehavior behavior);
}
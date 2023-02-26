using System.Data.Common;

namespace GGroupp.Infra.Sql.Api.Provider.Api.Test;

internal interface IStubDbConnection
{
    void Open();

    DbCommand CreateDbCommand();
}
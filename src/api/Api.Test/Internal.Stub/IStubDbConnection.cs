using System.Data.Common;

namespace GarageGroup.Infra.Sql.Api.Provider.Api.Test;

internal interface IStubDbConnection
{
    void Open();

    DbCommand CreateDbCommand();
}
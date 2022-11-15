using Microsoft.Data.SqlClient;
using System;

namespace GGroupp.Infra;

partial class MicrosoftDbProviderImpl
{
    public object GetSqlParameter(DbParameter parameter)
    {
        _ = parameter ?? throw new ArgumentNullException(nameof(parameter));
        return new SqlParameter(parameter.Name, parameter.Value);
    }
}
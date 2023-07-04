using Microsoft.Data.SqlClient;
using System;

namespace GarageGroup.Infra;

partial class MicrosoftDbProviderImpl
{
    public object GetSqlParameter(DbParameter parameter)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        return new SqlParameter
        {
            ParameterName = parameter.Name,
            Value = parameter.Value ?? DBNull.Value,
            IsNullable = parameter.Value is null
        };
    }
}
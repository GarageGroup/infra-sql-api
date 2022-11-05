using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace GGroupp.Infra;

partial class MicrosoftDbProviderImpl
{
    public object GetDbParameter(KeyValuePair<string, object?> parameter)
        =>
        new SqlParameter(parameter.Key, parameter.Value);
}
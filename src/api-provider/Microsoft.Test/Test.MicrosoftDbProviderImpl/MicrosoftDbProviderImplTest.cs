using System;
using Microsoft.Data.SqlClient;

namespace GGroupp.Infra.Sql.Api.Provider.Microsoft.Test;

public static partial class MicrosoftDbProviderImplTest
{
    private const string SomeConnectionString
        =
        "Server=someServerAddress;Database=someDataBase;User Id=someUser;Password=somePassword;";

    private static readonly SqlRetryLogicOption SomeRetryLogicOption
        =
        new()
        {
            NumberOfTries = 5,
            DeltaTime = TimeSpan.FromSeconds(3),
            MinTimeInterval = TimeSpan.FromSeconds(1),
            MaxTimeInterval = TimeSpan.FromSeconds(30),
            TransientErrors = new[] { 515 }
        };
}
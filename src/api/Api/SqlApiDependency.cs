using PrimeFuncPack;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GGroupp.Infra.Sql.Api.Provider.Api.Test")]

namespace GGroupp.Infra;

public static class SqlApiDependency
{
    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ISqlApi>(SqlApi.Create);
    }
}
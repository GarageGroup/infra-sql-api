using PrimeFuncPack;
using System;

namespace GGroupp.Infra;

public static class SqlApiDependency
{
    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider> dependency)
    {
        ArgumentNullException.ThrowIfNull(dependency);
        return dependency.Map<ISqlApi>(SqlApi.Create);
    }
}
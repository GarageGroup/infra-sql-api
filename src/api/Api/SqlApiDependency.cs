using PrimeFuncPack;
using System;

namespace GGroupp.Infra;

public static class SqlApiDependency
{
    public static Dependency<ISqlApi> UseSqlApi(this Dependency<IDbProvider> dependency)
    {
        _ = dependency ?? throw new ArgumentNullException(nameof(dependency));

        return dependency.Map<ISqlApi>(SqlApi.Create);
    }
}
using System;

namespace GGroupp.Infra;

public sealed partial class DbValue
{
    private readonly IDbValueProvider dbValueProvider;

    public DbValue(IDbValueProvider dbValueProvider)
        =>
        this.dbValueProvider = dbValueProvider ?? throw new ArgumentNullException(nameof(dbValueProvider));
}
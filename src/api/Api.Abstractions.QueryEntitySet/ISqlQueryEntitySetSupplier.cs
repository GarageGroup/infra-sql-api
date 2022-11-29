using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface ISqlQueryEntitySetSupplier
{
#if NET7_0_OR_GREATER

    ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(DbRequest request, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;

#else

    ValueTask<FlatArray<IDbItem>> QueryEntitySetAsync(DbRequest request, CancellationToken cancellationToken = default);

#endif
}
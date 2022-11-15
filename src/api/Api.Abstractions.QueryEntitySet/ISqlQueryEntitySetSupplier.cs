using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface ISqlQueryEntitySetSupplier
{
    ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(DbRequest request, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface ISqlQueryEntitySupplier
{
    ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(DbRequest request, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;
}
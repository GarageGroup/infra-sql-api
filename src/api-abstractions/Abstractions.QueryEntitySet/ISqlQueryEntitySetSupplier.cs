using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface ISqlQueryEntitySetSupplier
{
    ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;

    ValueTask<Result<FlatArray<T>, Failure<Unit>>> QueryEntitySetOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;
}
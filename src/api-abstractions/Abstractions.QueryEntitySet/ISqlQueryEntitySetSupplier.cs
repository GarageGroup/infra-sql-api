using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface ISqlQueryEntitySetSupplier
{
#if NET7_0_OR_GREATER
    ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;

    ValueTask<Result<FlatArray<T>, Failure<Unit>>> QueryEntitySetOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;
#else
    ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(
        IDbQuery query, Func<IDbItem, T> mapper, CancellationToken cancellationToken = default);

    ValueTask<Result<FlatArray<T>, Failure<Unit>>> QueryEntitySetOrFailureAsync<T>(
        IDbQuery query, Func<IDbItem, T> mapper, CancellationToken cancellationToken = default);
#endif
}
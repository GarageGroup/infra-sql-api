using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface ISqlQueryEntitySupplier
{
    ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;

    ValueTask<Result<T, Failure<EntityQueryFailureCode>>> QueryEntityOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;
}
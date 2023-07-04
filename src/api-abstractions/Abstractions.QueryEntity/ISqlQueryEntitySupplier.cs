using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface ISqlQueryEntitySupplier
{
#if NET7_0_OR_GREATER
    ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;
#else
    ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(
        IDbQuery query, Func<IDbItem, T> mapper, CancellationToken cancellationToken = default);
#endif
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface ISqlQueryEntitySupplier
{
#if NET7_0_OR_GREATER

    ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(DbRequest request, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>;

#else

    ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(
        DbRequest request, Func<IDbItem, T> mapper, CancellationToken cancellationToken = default);

#endif
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

public interface ISqlExecuteNonQuerySupplier
{
    ValueTask<int> ExecuteNonQueryAsync(
        IDbQuery query, CancellationToken cancellationToken = default);

    ValueTask<Result<int, Failure<Unit>>> ExecuteNonQueryOrFailureAsync(
        IDbQuery query, CancellationToken cancellationToken = default);
}
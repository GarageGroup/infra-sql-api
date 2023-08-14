using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi
{
    public ValueTask<Result<int, Failure<Unit>>> ExecuteNonQueryOrFailureAsync(
        IDbQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<int, Failure<Unit>>>(cancellationToken);
        }

        return InnerExecuteNonQueryOrFailureAsync(query, cancellationToken);
    }

    private async ValueTask<Result<int, Failure<Unit>>> InnerExecuteNonQueryOrFailureAsync(
        IDbQuery query, CancellationToken cancellationToken)
    {
        try
        {
            return await InnerExecuteNonQueryAsync(query, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not TaskCanceledException)
        {
            return exception.ToFailure("An unexpected exception was thrown when executing the input query");
        }
    }
}
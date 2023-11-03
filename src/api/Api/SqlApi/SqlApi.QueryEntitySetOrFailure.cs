using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi
{
    public ValueTask<Result<FlatArray<T>, Failure<Unit>>> QueryEntitySetOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(query);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<FlatArray<T>, Failure<Unit>>>(cancellationToken);
        }

        return InnerQueryEntitySetOrFailureAsync<T>(query, cancellationToken);
    }

    private async ValueTask<Result<FlatArray<T>, Failure<Unit>>> InnerQueryEntitySetOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken)
        where T : IDbEntity<T>
    {
        try
        {
            return await InnerQueryEntitySetAsync<T>(query, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return exception.ToFailure("An unexpected exception was thrown when executing the input query");
        }
    }
}
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

        return InnerQueryEntitySetOrFailureAsync<T>(query, T.ReadEntity, cancellationToken);
    }

    private async ValueTask<Result<FlatArray<T>, Failure<Unit>>> InnerQueryEntitySetOrFailureAsync<T>(
        IDbQuery query, Func<IDbItem, T> mapper, CancellationToken cancellationToken)
    {
        try
        {
            return await InnerQueryEntitySetAsync(query, mapper, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return exception.ToFailure("An unexpected exception was thrown when executing the input query");
        }
    }
}
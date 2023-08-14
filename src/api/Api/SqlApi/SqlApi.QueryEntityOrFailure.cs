using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi
{
#if NET7_0_OR_GREATER
    public ValueTask<Result<T, Failure<EntityQueryFailureCode>>> QueryEntityOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(query);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Failure<EntityQueryFailureCode>>>(cancellationToken);
        }

        return InnerQueryEntityOrFailureAsync(query, T.ReadEntity, cancellationToken);
    }
#else
    public ValueTask<Result<T, Failure<EntityQueryFailureCode>>> QueryEntityOrFailureAsync<T>(
        IDbQuery query, Func<IDbItem, T> mapper, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(mapper);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Failure<EntityQueryFailureCode>>>(cancellationToken);
        }

        return InnerQueryEntityOrFailureAsync(query, mapper, cancellationToken);
    }
#endif

    private async ValueTask<Result<T, Failure<EntityQueryFailureCode>>> InnerQueryEntityOrFailureAsync<T>(
        IDbQuery query, Func<IDbItem, T> mapper, CancellationToken cancellationToken)
    {
        try
        {
            var result = await InnerQueryDbItemOrAbsentAsync(query, mapper, cancellationToken).ConfigureAwait(false);
            return result.MapFailure(NotFoundFailure);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return exception.ToFailure(EntityQueryFailureCode.Unknown, "An unexpected exception was thrown when executing the input query");
        }

        static Failure<EntityQueryFailureCode> NotFoundFailure(Unit _)
            =>
            Failure.Create(EntityQueryFailureCode.NotFound, "A db entity was not found by the input query");
    }
}
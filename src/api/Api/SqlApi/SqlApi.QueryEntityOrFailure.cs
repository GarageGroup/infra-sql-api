using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi
{
    public ValueTask<Result<T, Failure<EntityQueryFailureCode>>> QueryEntityOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(query);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Failure<EntityQueryFailureCode>>>(cancellationToken);
        }

        return InnerQueryEntityOrFailureAsync<T>(query, cancellationToken);
    }

    private async ValueTask<Result<T, Failure<EntityQueryFailureCode>>> InnerQueryEntityOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken)
        where T : IDbEntity<T>
    {
        try
        {
            var result = await InnerQueryDbItemOrAbsentAsync<T>(query, cancellationToken).ConfigureAwait(false);
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
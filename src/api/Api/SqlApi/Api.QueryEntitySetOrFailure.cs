using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi<TDbConnection>
{
    public async ValueTask<Result<FlatArray<T>, Failure<Unit>>> QueryEntitySetOrFailureAsync<T>(
        IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            return await InnerQueryEntitySetAsync<T>(query, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return exception.ToFailure("An unexpected exception was thrown when executing the input database query");
        }
    }
}
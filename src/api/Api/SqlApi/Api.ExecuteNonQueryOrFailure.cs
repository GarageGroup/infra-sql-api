using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi<TDbConnection>
{
    public async ValueTask<Result<int, Failure<Unit>>> ExecuteNonQueryOrFailureAsync(
        IDbQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        try
        {
            return await InnerExecuteNonQueryAsync(query, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return exception.ToFailure("An unexpected exception was thrown when executing the input database query");
        }
    }
}
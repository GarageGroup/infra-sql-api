using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi<TDbConnection>
{
    public async ValueTask<Result<Unit, Failure<Unit>>> PingAsync(Unit input, CancellationToken cancellationToken)
    {
        try
        {
            using var dbConnection = dbProvider.GetDbConnection();
            await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

            using var dbCommand = dbProvider.GetDbCommand(dbConnection, PingQuery, default, default);
            _ = await dbCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

            return Result.Success<Unit>(default);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            return exception.ToFailure("An unexpected exception was thrown when trying to ping a database");
        }
    }
}
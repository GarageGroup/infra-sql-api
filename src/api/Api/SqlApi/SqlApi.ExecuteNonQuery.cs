using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi
{
    public ValueTask<int> ExecuteNonQueryAsync(IDbQuery query, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<int>(cancellationToken);
        }

        return InnerExecuteNonQueryAsync(query, cancellationToken);
    }

    private async ValueTask<int> InnerExecuteNonQueryAsync(IDbQuery query, CancellationToken cancellationToken)
    {
        using var dbConnection = dbProvider.GetDbConnection();
        await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        var dbCommand = CreateDbCommand(dbConnection, query);
        return await dbCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
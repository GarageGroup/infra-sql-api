using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class SqlApi
{
    public ValueTask<int> ExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken = default)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<int>(cancellationToken);
        }

        return InnerExecuteNonQueryAsync(request, cancellationToken);
    }

    private async ValueTask<int> InnerExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken)
    {
        using var dbConnection = dbProvider.GetDbConnection();
        await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        var dbCommand = CreateDbCommand(dbConnection, request);
        return await dbCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }
}
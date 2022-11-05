using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class SqlApi
{
    public ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(SqlRequest request, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Unit>>(cancellationToken);
        }

        return InnerQueryEntityOrAbsentAsync<T>(request, cancellationToken);
    }

    private async ValueTask<Result<T, Unit>> InnerQueryEntityOrAbsentAsync<T>(SqlRequest request, CancellationToken cancellationToken)
        where T : IDbEntity<T>
    {
        using var dbConnection = dbProvider.GetDbConnection();
        await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var dbCommand = CreateDbCommand(dbConnection, request);
        using var dbReader = await dbCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var isPresent = await dbReader.ReadAsync(cancellationToken).ConfigureAwait(false);

        if (isPresent is false)
        {
            return default;
        }

        var dbItem = new DbItem(dbReader);
        return T.ReadEntity(dbItem);
    }
}
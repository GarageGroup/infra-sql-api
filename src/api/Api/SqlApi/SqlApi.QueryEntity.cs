using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi
{
    public ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(query);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Unit>>(cancellationToken);
        }

        return InnerQueryDbItemOrAbsentAsync<T>(query, cancellationToken);
    }

    private async ValueTask<Result<T, Unit>> InnerQueryDbItemOrAbsentAsync<T>(
        IDbQuery query, CancellationToken cancellationToken)
        where T : IDbEntity<T>
    {
        using var dbConnection = dbProvider.GetDbConnection();
        await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var dbCommand = CreateDbCommand(dbConnection, query);
        using var dbReader = await dbCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var isPresent = await dbReader.ReadAsync(cancellationToken).ConfigureAwait(false);

        if (isPresent is false)
        {
            return default;
        }

        var fieldIndexes = CreateFieldIndexes(dbReader);
        var dbItem = new DbItem(dbReader, fieldIndexes);

        return T.ReadEntity(dbItem);
    }
}
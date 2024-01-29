using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi<TDbConnection>
{
    public ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(query);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<FlatArray<T>>(cancellationToken);
        }

        return InnerQueryEntitySetAsync<T>(query, cancellationToken);
    }

    private async ValueTask<FlatArray<T>> InnerQueryEntitySetAsync<T>(
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

        var collection = new List<T>();
        var fieldIndexes = CreateFieldIndexes(dbReader);

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var dbItem = new DbItem(dbReader, fieldIndexes);
            var dbEntity = T.ReadEntity(dbItem);

            collection.Add(dbEntity);

            if (await dbReader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                continue;
            }

            return collection.ToFlatArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class SqlApi
{
#if NET7_0_OR_GREATER

    public ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(DbRequest request, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(request);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<FlatArray<T>>(cancellationToken);
        }

        return InnerQueryEntitySetAsync<T>(request, T.ReadEntity, cancellationToken);
    }

#else

    public ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(
        DbRequest request, Func<IDbItem, T> mapper, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(mapper);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<FlatArray<T>>(cancellationToken);
        }

        return InnerQueryEntitySetAsync(request, mapper, cancellationToken);
    }

#endif

    private async ValueTask<FlatArray<T>> InnerQueryEntitySetAsync<T>(
        DbRequest request, Func<IDbItem, T> mapper, CancellationToken cancellationToken)
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

        var collection = new List<T>();
        var fieldIndexes = CreateFieldIndexes(dbReader);

        while (true)
        {
            var dbItem = new DbItem(dbReader, fieldIndexes);
            var dbEntity = mapper.Invoke(dbItem);

            collection.Add(dbEntity);

            if (await dbReader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                continue;
            }

            return collection.ToArray();
        }
    }
}
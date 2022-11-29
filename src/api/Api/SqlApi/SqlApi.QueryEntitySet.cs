using System;
using System.Collections.Generic;
using System.Linq;
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

        return InnerQueryEntitySetAsync<T>(request, cancellationToken);
    }

#else

    public ValueTask<FlatArray<IDbItem>> QueryEntitySetAsync(DbRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<FlatArray<IDbItem>>(cancellationToken);
        }

        return InnerQueryDbItemSetAsync(request, cancellationToken);
    }

#endif

#if NET7_0_OR_GREATER

    private async ValueTask<FlatArray<T>> InnerQueryEntitySetAsync<T>(DbRequest request, CancellationToken cancellationToken)
        where T : IDbEntity<T>
    {
        var dbItems = await InnerQueryDbItemSetAsync(request, cancellationToken).ConfigureAwait(false);
        return MapDbItems<T>(dbItems);
    }

    private static FlatArray<T> MapDbItems<T>(FlatArray<IDbItem> dbItems)
        where T : IDbEntity<T>
    {
        if (dbItems.IsEmpty)
        {
            return default;
        }

        var builder = FlatArray<T>.Builder.OfLength(dbItems.Length);
        var index = 0;

        foreach (var dbItem in dbItems)
        {
            builder[index] = T.ReadEntity(dbItem);
            index++;
        }

        return builder.MoveToArray();
    }

#endif

    private async ValueTask<FlatArray<IDbItem>> InnerQueryDbItemSetAsync(DbRequest request, CancellationToken cancellationToken)
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

        var collection = new List<IDbItem>();
        var fieldIndexes = CreateFieldIndexes(dbReader);

        while (true)
        {
            var dbItem = new DbItem(dbReader, fieldIndexes);
            collection.Add(dbItem);

            if (await dbReader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                continue;
            }

            return collection.ToArray();
        }
    }
}
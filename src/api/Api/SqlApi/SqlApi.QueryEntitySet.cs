using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class SqlApi
{
    public ValueTask<FlatArray<T>> QueryEntitySetAsync<T>(SqlRequest request, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<FlatArray<T>>(cancellationToken);
        }

        return InnreQueryEntitySetAsync<T>(request, cancellationToken);
    }

    private async ValueTask<FlatArray<T>> InnreQueryEntitySetAsync<T>(SqlRequest request, CancellationToken cancellationToken)
        where T : IDbEntity<T>
    {
        using var dbConnection = dbProvider.GetDbConnection();
        await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        using var dbCommand = CreateDbCommand(dbConnection, request);
        using var dbReader = await dbCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

        var isPresent = await dbReader.ReadAsync(cancellationToken).ConfigureAwait(false);

        if (isPresent is false)
        {
            return FlatArray.Empty<T>();
        }

        var collection = new List<T>();

        do
        {
            var dbItem = new DbItem(dbReader);
            var entity = T.ReadEntity(dbItem);

            collection.Add(entity);
            isPresent = await dbReader.ReadAsync(cancellationToken).ConfigureAwait(false);
        }
        while (isPresent);

        return collection.ToFlatArray();
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GarageGroup.Infra;

partial class SqlApi
{
#if NET7_0_OR_GREATER
    public ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(IDbQuery query, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(query);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Unit>>(cancellationToken);
        }

        return InnerQueryDbItemOrAbsentAsync(query, T.ReadEntity, cancellationToken);
    }
#else
    public ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(
        IDbQuery query, Func<IDbItem, T> mapper, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(mapper);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Unit>>(cancellationToken);
        }

        return InnerQueryDbItemOrAbsentAsync(query, mapper, cancellationToken);
    }
#endif

    private async ValueTask<Result<T, Unit>> InnerQueryDbItemOrAbsentAsync<T>(
        IDbQuery query, Func<IDbItem, T> mapper, CancellationToken cancellationToken)
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

        return mapper.Invoke(dbItem);
    }
}
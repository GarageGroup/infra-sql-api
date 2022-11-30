using System;
using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

partial class SqlApi
{
#if NET7_0_OR_GREATER

    public ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(DbRequest request, CancellationToken cancellationToken = default)
        where T : IDbEntity<T>
    {
        ArgumentNullException.ThrowIfNull(request);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Unit>>(cancellationToken);
        }

        return InnerQueryDbItemOrAbsentAsync(request, T.ReadEntity, cancellationToken);
    }

#else

    public ValueTask<Result<T, Unit>> QueryEntityOrAbsentAsync<T>(
        DbRequest request, Func<IDbItem, T> mapper, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(mapper);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<T, Unit>>(cancellationToken);
        }

        return InnerQueryDbItemOrAbsentAsync(request, mapper, cancellationToken);
    }

#endif

    private async ValueTask<Result<T, Unit>> InnerQueryDbItemOrAbsentAsync<T>(
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

        var fieldIndexes = CreateFieldIndexes(dbReader);
        var dbItem = new DbItem(dbReader, fieldIndexes);

        return mapper.Invoke(dbItem);
    }
}
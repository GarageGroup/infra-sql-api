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

        return InnerQueryEntityOrAbsentAsync<T>(request, cancellationToken);
    }

#else

    public ValueTask<Result<IDbItem, Unit>> QueryEntityOrAbsentAsync(DbRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<Result<IDbItem, Unit>>(cancellationToken);
        }

        return InnerQueryDbItemOrAbsentAsync(request, cancellationToken);
    }

#endif

#if NET7_0_OR_GREATER

    private async ValueTask<Result<T, Unit>> InnerQueryEntityOrAbsentAsync<T>(DbRequest request, CancellationToken cancellationToken)
        where T : IDbEntity<T>
    {
        var result = await InnerQueryDbItemOrAbsentAsync(request, cancellationToken).ConfigureAwait(false);
        return result.MapSuccess(T.ReadEntity);
    }

#endif

    private async ValueTask<Result<IDbItem, Unit>> InnerQueryDbItemOrAbsentAsync(DbRequest request, CancellationToken cancellationToken)
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
        return new DbItem(dbReader, fieldIndexes);
    }
}
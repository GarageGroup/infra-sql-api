using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface ISqlExecuteNonQuerySupplier
{
    ValueTask<int> ExecuteNonQueryAsync(IDbQuery query, CancellationToken cancellationToken = default);
}
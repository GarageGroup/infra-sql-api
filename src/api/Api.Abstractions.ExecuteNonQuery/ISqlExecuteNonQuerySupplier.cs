using System.Threading;
using System.Threading.Tasks;

namespace GGroupp.Infra;

public interface ISqlExecuteNonQuerySupplier
{
    ValueTask<int> ExecuteNonQueryAsync(SqlRequest request, CancellationToken cancellationToken = default);
}
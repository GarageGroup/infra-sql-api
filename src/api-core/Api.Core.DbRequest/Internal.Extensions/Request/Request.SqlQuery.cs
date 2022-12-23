using System.Text;

namespace GGroupp.Infra;

partial class DbRequestExtensions
{
    internal static string BuildSqlQuery(this DbRequest request)
    {
        var queryBuilder = new StringBuilder("SELECT ");

        if (request.Top is not null && request.Offset is null)
        {
            queryBuilder = queryBuilder.Append("TOP ").Append(request.Top.Value).Append(' ');
        }

        if (request.SelectedFields.IsEmpty)
        {
            queryBuilder = queryBuilder.Append('*');
        }
        else
        {
            queryBuilder = queryBuilder.AppendJoined(request.SelectedFields.AsEnumerable());
        }

        queryBuilder = queryBuilder.Append(" FROM ").Append(request.TableName);
        if (string.IsNullOrWhiteSpace(request.ShortName) is false)
        {
            queryBuilder = queryBuilder.Append(' ').Append(request.ShortName);
        }

        var joinQuery = request.JoinedTables.BuildSqlQuery();
        if (string.IsNullOrWhiteSpace(joinQuery) is false)
        {
            queryBuilder = queryBuilder.Append(' ').Append(joinQuery);
        }

        var filterQuery = request.Filters.BuildSqlQuery();
        if (string.IsNullOrWhiteSpace(filterQuery) is false)
        {
            queryBuilder = queryBuilder.Append(" WHERE ").Append(filterQuery);
        }

        var orderByQuery = request.Orders.BuildSqlQuery();
        if (string.IsNullOrWhiteSpace(orderByQuery) is false)
        {
            queryBuilder = queryBuilder.Append(" ORDER BY ").Append(orderByQuery);
        }

        if (request.Offset is null)
        {
            return queryBuilder.ToString();
        }

        queryBuilder = queryBuilder.Append(" OFFSET ").Append(request.Offset.Value);
        if (request.Top is null)
        {
            return queryBuilder.ToString();
        }

        return queryBuilder.Append(" ROWS FETCH NEXT ").Append(request.Top.Value).Append(" ROWS ONLY").ToString();
    }
}
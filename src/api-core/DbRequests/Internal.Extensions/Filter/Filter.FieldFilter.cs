using System.Text;

namespace GGroupp.Infra;

partial class DbQueryExtensions
{
    internal static string BuildFilterSqlQuery(this DbFieldFilter filter)
        =>
        new StringBuilder()
        .Append(filter.FieldName)
        .Append(' ')
        .Append(filter.Operator.GetSign())
        .Append(' ')
        .Append(filter.RawFieldValue)
        .ToString();
}
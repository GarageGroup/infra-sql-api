using System.Text;

namespace GarageGroup.Infra;

internal static partial class DbEntityBuilder
{
    private const string InnerQueryBuilderClassName = "InnerQueryBuilder";

    private const string DbExtensionFieldVariableName = "dbExtensionFieldValues";

    private static string GetQueryBuildMethodName(this DbSelectQueryData queryData)
        =>
        "Build" + queryData.QueryName;

    private static string BuildHeaderLine(this DbEntityMetadata metadata)
    {
        var builder = new StringBuilder("partial ");

        if (metadata.EntityType.IsRecordType)
        {
            builder = builder.Append("record ");
        }

        if (metadata.EntityType.IsValueType)
        {
            builder = builder.Append("struct");
        }
        else
        {
            builder = builder.Append("class");
        }
        return builder.Append(' ').Append(metadata.EntityType.DisplayedData.DisplayedTypeName).ToString();
    }
}
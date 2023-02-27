using System.Text;

namespace GGroupp.Infra;

internal static partial class DbEntityBuilder
{
    private static string BuildHeaderLine(this DbEntityMetadata metadata)
    {
        var builder = new StringBuilder("partial ");

        if (metadata.IsRecordType)
        {
            builder = builder.Append("record ");
        }

        if (metadata.IsValueType)
        {
            builder = builder.Append("struct");
        }
        else
        {
            builder = builder.Append("class");
        }
        return builder.Append(' ').Append(metadata.EntityType.DisplayedTypeName).ToString();
    }

    private static string AsStringSourceCode(this string? source, string defaultSourceCode = "string.Empty")
        =>
        string.IsNullOrEmpty(source) ? defaultSourceCode : $"\"{source.EncodeString()}\"";

    private static string? EncodeString(this string? source)
        =>
        source?.Replace("\"", "\\\"");
}
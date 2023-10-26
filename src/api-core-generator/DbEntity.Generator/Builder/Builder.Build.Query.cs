using System.Linq;
using System.Text;

namespace GarageGroup.Infra;

partial class DbEntityBuilder
{
    internal static string BuildQuerySourceCode(this DbEntityMetadata metadata)
        =>
        new SourceBuilder(
            metadata.EntityType.DisplayedData.AllNamespaces.FirstOrDefault())
        .AddUsings(
            metadata.EntityType.DisplayedData.AllNamespaces)
        .AddUsing(
            "System",
            "GarageGroup.Infra")
        .AppendCodeLine(
            metadata.BuildHeaderLine())
        .BeginCodeBlock()
        .AppendQueries(metadata)
        .AppendEmptyLine()
        .AppendDirective(
            "#if NET6_0")
        .AppendCodeLine(
            $"private static class {InnerQueryBuilderClassName}")
        .AppendInnerQueryBuilderBody(metadata)
        .AppendDirective(
            "#endif")
        .EndCodeBlock()
        .AppendEmptyLine()
        .AppendDirective(
            "#if NET7_0_OR_GREATER")
        .AppendCodeLine(
            $"file static class {InnerQueryBuilderClassName}")
        .AppendInnerQueryBuilderBody(metadata)
        .AppendDirective(
            "#endif")
        .Build();

    private static SourceBuilder AppendQueries(this SourceBuilder builder, DbEntityMetadata metadata)
    {
        if (metadata.SelectQueries.Any() is false)
        {
            return builder;
        }

        var codeLines = metadata.SelectQueries.Select(BuildCodeLine).ToArray();
        for (var i = 0; i < codeLines.Length; i++)
        {
            builder = builder.AppendCodeLine(codeLines[i]);

            if (i != codeLines.Length - 1)
            {
                builder = builder.AppendEmptyLine();
            }
        }

        return builder;

        static string BuildCodeLine(DbSelectQueryData query)
            =>
            new StringBuilder("internal static DbSelectQuery ")
            .Append(query.QueryName)
            .Append(" { get; } = ")
            .Append(InnerQueryBuilderClassName)
            .Append('.')
            .Append(
                query.GetQueryBuildMethodName())
            .Append("();")
            .ToString();
    }

    private static SourceBuilder AppendInnerQueryBuilderBody(this SourceBuilder builder, DbEntityMetadata metadata)
    {
        builder = builder.BeginCodeBlock();

        for (var i = 0; i < metadata.SelectQueries.Count; i++)
        {
            if (i > 0)
            {
                builder = builder.AppendEmptyLine();
            }

            var query = metadata.SelectQueries[i];
            builder = builder.AppendQueryBuildMethod(query);
        }

        return builder.EndCodeBlock();
    }

    private static SourceBuilder AppendQueryBuildMethod(this SourceBuilder builder, DbSelectQueryData queryData)
    {
        builder = builder.AppendCodeLine($"public static DbSelectQuery {queryData.GetQueryBuildMethodName()}()").BeginCodeBlock();
        var hasJoinedTables = queryData.JoinedTables.Any();

        if (queryData.FieldNames.Any() is false && hasJoinedTables is false)
        {
            return builder.AppendCodeLine($"return new({queryData.BuildDbSelectQueryArguments()});").EndCodeBlock();
        }

        builder = builder.AppendCodeLine($"return new({queryData.BuildDbSelectQueryArguments()})").BeginCodeBlock();

        if (queryData.FieldNames.Count is 1)
        {
            builder = builder.AppendCodeLine($"SelectedFields = new({queryData.FieldNames[0].AsStringSourceCodeOrStringEmpty()}),");
        }
        else if (queryData.FieldNames.Count > 1)
        {
            builder = builder.AppendCodeLine("SelectedFields = InnerBuildSelectedFields(),");
        }

        if (hasJoinedTables)
        {
            builder = builder.AppendCodeLine("JoinedTables = InnerBuildJoinedTables(),");
        }

        if (queryData.GroupByFields.Count is 1)
        {
            builder = builder.AppendCodeLine($"GroupByFields = new({queryData.GroupByFields[0].AsStringSourceCodeOrStringEmpty()})");
        }
        else if (queryData.GroupByFields.Count > 1)
        {
            builder = builder.AppendCodeLine("GroupByFields = InnerBuildGroupByFields()");
        }

        builder = builder.EndCodeBlock(';');

        if (queryData.FieldNames.Count > 1)
        {
            builder = builder
                .AppendEmptyLine()
                .AppendCodeLine("static FlatArray<string> InnerBuildSelectedFields()")
                .BeginCodeBlock()
                .AppendCodeLine($"var builder = FlatArray<string>.Builder.OfLength({queryData.FieldNames.Count});");

            for (var i = 0; i < queryData.FieldNames.Count; i++)
            {
                builder = builder.AppendCodeLine($"builder[{i}] = {queryData.FieldNames[i].AsStringSourceCodeOrStringEmpty()};");
            }

            builder = builder.AppendCodeLine("return builder.MoveToFlatArray();").EndCodeBlock();
        }

        if (hasJoinedTables)
        {
            builder = builder
                .AppendEmptyLine()
                .AppendCodeLine("static FlatArray<DbJoinedTable> InnerBuildJoinedTables()")
                .BeginCodeBlock()
                .AppendCodeLine($"var builder = FlatArray<DbJoinedTable>.Builder.OfLength({queryData.JoinedTables.Count});");

            for (var i = 0; i < queryData.JoinedTables.Count; i++)
            {
                builder = builder.AppendCodeLine($"builder[{i}] = {queryData.JoinedTables[i].BuildDbJoinedTableSourceCode()};");
            }

            builder = builder.AppendCodeLine("return builder.MoveToFlatArray();").EndCodeBlock();
        }

        if (queryData.GroupByFields.Count > 1)
        {
            builder = builder
                .AppendEmptyLine()
                .AppendCodeLine("static FlatArray<string> InnerBuildGroupByFields()")
                .BeginCodeBlock()
                .AppendCodeLine($"var builder = FlatArray<string>.Builder.OfLength({queryData.GroupByFields.Count});");

            for (var i = 0; i < queryData.GroupByFields.Count; i++)
            {
                builder = builder.AppendCodeLine($"builder[{i}] = {queryData.GroupByFields[i].AsStringSourceCodeOrStringEmpty()};");
            }

            builder = builder.AppendCodeLine("return builder.MoveToFlatArray();").EndCodeBlock();
        }

        return builder.EndCodeBlock();
    }

    private static string BuildDbJoinedTableSourceCode(this DbJoinData dbJoinData)
    {
        var builder = new StringBuilder("new(");

        builder = dbJoinData.JoinType switch
        {
            0 => builder.Append("DbJoinType.Inner"),
            1 => builder.Append("DbJoinType.Left"),
            2 => builder.Append("DbJoinType.Right"),
            var joinType => builder.Append($"(DbJoinType){joinType}")
        };

        return builder
            .Append(", ")
            .Append(dbJoinData.TableName.AsStringSourceCodeOrStringEmpty())
            .Append(", ")
            .Append(dbJoinData.TableAlias.AsStringSourceCodeOrStringEmpty())
            .Append(", ")
            .Append("new DbRawFilter(")
            .Append(dbJoinData.RawFilter.AsStringSourceCodeOrStringEmpty())
            .Append("))")
            .ToString();
    }

    private static string BuildDbSelectQueryArguments(this DbSelectQueryData queryData)
    {
        var builder = new StringBuilder(queryData.TableData.TableName.AsStringSourceCodeOrStringEmpty());
        if (string.IsNullOrEmpty(queryData.TableData.TableAlias))
        {
            return builder.ToString();
        }

        return builder.Append(", ").Append(queryData.TableData.TableAlias.AsStringSourceCodeOrStringEmpty()).ToString();
    }
}
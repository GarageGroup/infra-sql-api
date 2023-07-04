using System.Linq;
using Microsoft.CodeAnalysis;

namespace GarageGroup.Infra;

[Generator]
internal sealed class DbEntitySourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var dbEntityType in context.GetDbEntityTypes())
        {
            var readEntitySourceCode = dbEntityType.BuildReadEntitySourceCode();
            context.AddSource($"{dbEntityType.FileName}.ReadEntity.g.cs", readEntitySourceCode);

            if (dbEntityType.SelectQueries.Any() is false)
            {
                continue;
            }

            var querySourceCode = dbEntityType.BuildQuerySourceCode();
            context.AddSource($"{dbEntityType.FileName}.Query.g.cs", querySourceCode);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }
}
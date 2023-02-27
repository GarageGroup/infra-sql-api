using Microsoft.CodeAnalysis;

namespace GGroupp.Infra;

[Generator]
internal sealed class DbEntitySourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        foreach (var dbEntityType in context.GetDbEntityTypes())
        {
            var constructorSourceCode = dbEntityType.BuildSourceCode();
            context.AddSource($"{dbEntityType.FileName}.g.cs", constructorSourceCode);
        }
    }

    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this one
    }
}
using System;
using System.Collections.Generic;

namespace GarageGroup.Infra;

internal sealed record class DisplayedMethodData
{
    public DisplayedMethodData(IReadOnlyCollection<string> allNamespaces, string sourceCode)
    {
        AllNamespaces = allNamespaces ?? Array.Empty<string>();
        SourceCode = sourceCode ?? string.Empty;
    }

    public IReadOnlyCollection<string> AllNamespaces { get; }

    public string SourceCode { get; }
}
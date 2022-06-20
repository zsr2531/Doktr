using System.Diagnostics.CodeAnalysis;
using AsmResolver.DotNet;
using Doktr.Lifters.Common.DependencyGraph;

namespace Doktr.Lifters.AsmResolver.DependencyGraph;

public static class DependencyGraphExtensions
{
    public static bool AddMethodDependency(
        this DependencyGraph<IMemberDefinition> depGraph,
        MethodDefinition from,
        MethodDefinition to,
        DependencyEdgeKind kind)
    {
        if (TryExtractSemantics(from, to, out var fromAssoc, out var toAssoc))
            depGraph.AddDependency(fromAssoc, toAssoc, kind);

        return depGraph.AddDependency(from, to, kind);
    }

    private static bool TryExtractSemantics(
        MethodDefinition from,
        MethodDefinition to,
        [NotNullWhen(true)] out IHasSemantics? fromAssoc,
        [NotNullWhen(true)] out IHasSemantics? toAssoc)
    {
        if (from.Semantics?.Association is { } f && to.Semantics?.Association is { } t)
        {
            fromAssoc = f;
            toAssoc = t;
            return true;
        }

        fromAssoc = null;
        toAssoc = null;
        return false;
    }
}
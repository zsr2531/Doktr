using System.Diagnostics.CodeAnalysis;
using AsmResolver.DotNet;
using Doktr.Lifters.Common.DependencyGraph;
using Serilog;

namespace Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;

public class BaseTypeAnalyzer : IDependencyGraphAnalyzer<IMemberDefinition>
{
    private readonly ILogger _logger;

    public BaseTypeAnalyzer(ILogger logger)
    {
        _logger = logger;
    }

    public void AnalyzeNode(DependencyNode<IMemberDefinition> node)
    {
        if (node.Value is not TypeDefinition type)
            return;

        var depGraph = node.ParentGraph;
        if (TryResolveBaseType(type, out var baseType))
            depGraph.AddDependency(type, baseType, DependencyEdgeKind.Extension);

        foreach (var inf in ResolveInterfaces(type))
            depGraph.AddDependency(type, inf, DependencyEdgeKind.Implementation);
    }

    private bool TryResolveBaseType(TypeDefinition type, [NotNullWhen(true)] out TypeDefinition? baseType)
    {
        baseType = null;
        var candidate = type.BaseType;
        if (candidate is null)
        {
            _logger.Verbose("{Type} has no base type", type);
            return false;
        }

        var resolved = candidate.Resolve();
        if (resolved is null)
        {
            _logger.Warning("Failed to resolve the base type of {Type} (base: {BaseType})", type, baseType);
            return false;
        }

        baseType = resolved;
        return true;
    }

    private List<TypeDefinition> ResolveInterfaces(TypeDefinition type)
    {
        var interfaces = new List<TypeDefinition>();
        foreach (var impl in type.Interfaces)
        {
            var inf = impl.Interface!;
            var resolved = inf.Resolve();
            if (resolved is null)
            {
                _logger.Warning("Failed to resolve interface of {Type} (interface: {Interface})", type, inf);
                continue;
            }

            interfaces.Add(resolved);
        }

        return interfaces;
    }
}
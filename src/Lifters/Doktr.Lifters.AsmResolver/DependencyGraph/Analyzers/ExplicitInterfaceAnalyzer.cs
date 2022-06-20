using AsmResolver.DotNet;
using Doktr.Lifters.Common.DependencyGraph;
using Serilog;

namespace Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;

public class ExplicitInterfaceAnalyzer : IDependencyGraphAnalyzer<IMemberDefinition>
{
    private readonly ILogger _logger;

    public ExplicitInterfaceAnalyzer(ILogger logger)
    {
        _logger = logger;
    }

    public void AnalyzeNode(DependencyNode<IMemberDefinition> node)
    {
        var depGraph = node.ParentGraph;
        var member = node.Value;
        if (member is not TypeDefinition { MethodImplementations.Count: >0 } type)
            return;

        var impls = type.MethodImplementations;
        foreach (var impl in impls)
            AnalyzeImplementation(depGraph, impl);
    }

    private void AnalyzeImplementation(DependencyGraph<IMemberDefinition> depGraph, MethodImplementation impl)
    {
        var method = impl.Body?.Resolve();
        var decl = impl.Declaration?.Resolve();
        if (method is null || decl is null)
        {
            _logger.Warning("Failed to resolve explicit implementation {Impl}", impl);
            return;
        }

        _logger.Verbose("{Implementation} explicitly implements {Declaration}", method, decl);
        depGraph.AddMethodDependency(method, decl, DependencyEdgeKind.ExplicitImplementation);
    }
}
using System.Diagnostics.CodeAnalysis;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using Doktr.Lifters.Common.DependencyGraph;
using Serilog;

namespace Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;

public class ImplicitInterfaceAnalyzer : IDependencyGraphAnalyzer<IMemberDefinition>
{
    private static readonly GenericMethodSignatureComparer Comparer = GenericMethodSignatureComparer.Instance;
    private readonly ILogger _logger;

    public ImplicitInterfaceAnalyzer(ILogger logger)
    {
        _logger = logger;
    }

    public void AnalyzeNode(DependencyNode<IMemberDefinition> node)
    {
        var member = node.Value;
        if (member is not TypeDefinition { IsInterface: false, Interfaces.Count: > 0 } type)
            return;

        AnalyzeInterfaces(node.ParentGraph, type);
    }

    private void AnalyzeInterfaces(DependencyGraph<IMemberDefinition> depGraph, TypeDefinition type)
    {
        foreach (var impl in type.Interfaces)
            AnalyzeImplementation(depGraph, type, impl.Interface!);
    }

    private void AnalyzeImplementation(
        DependencyGraph<IMemberDefinition> depGraph,
        TypeDefinition implementor,
        ITypeDefOrRef inf)
    {
        var resolved = inf.Resolve();
        if (resolved is null)
        {
            _logger.Warning("Failed to resolve interface {Interface}", inf);
            return;
        }

        foreach (var method in resolved.Methods.Where(m => !m.IsStatic))
        {
            if (!TryFindImplementation(implementor, inf, method, out var impl))
            {
                _logger.Warning("Failed to find implementation of {Method} in {Implementor}", method, implementor);
                continue;
            }

            depGraph.AddMethodDependency(impl, method, DependencyEdgeKind.Implementation);
            _logger.Verbose("{Implementor} implements {Method}", impl, method);
        }
    }

    private static bool TryFindImplementation(
        TypeDefinition implementor,
        ITypeDefOrRef inf,
        MethodDefinition needle,
        [NotNullWhen(true)] out MethodDefinition? implementation)
    {
        implementation = null;
        var infSig = inf.ToTypeSignature();

        foreach (var method in implementor.Methods)
        {
            if (!Implements(method, needle, infSig))
                continue;

            implementation = method;
            return true;
        }

        return false;

        static bool Implements(MethodDefinition candidate, MethodDefinition decl, TypeSignature genericProvider)
        {
            if (candidate.IsStatic || candidate.Name != decl.Name)
                return false;

            return Comparer.Equals(candidate.Signature!, decl.Signature!, genericProvider);
        }
    }
}
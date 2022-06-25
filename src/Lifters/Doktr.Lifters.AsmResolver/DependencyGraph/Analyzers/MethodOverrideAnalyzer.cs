using System.Diagnostics.CodeAnalysis;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using Doktr.Lifters.Common.DependencyGraph;
using Serilog;

namespace Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;

public class MethodOverrideAnalyzer : IDependencyGraphAnalyzer<IMemberDefinition>
{
    private static readonly GenericMethodSignatureComparer Comparer = GenericMethodSignatureComparer.Instance;
    private readonly ILogger _logger;

    public MethodOverrideAnalyzer(ILogger logger)
    {
        _logger = logger;
    }

    public void AnalyzeNode(DependencyNode<IMemberDefinition> node)
    {
        if (node.Value is not MethodDefinition method)
            return;
        if (!HasOverride(method, out var baseType))
            return;
        if (!TryWalkInheritance(method, baseType, out var baseMethod))
            return;

        node.ParentGraph.AddMethodDependency(method, baseMethod, DependencyEdgeKind.Override);
        _logger.Verbose("{Override} overrides {BaseMethod}", method, baseMethod);

        static bool HasOverride(MethodDefinition method, out ITypeDefOrRef? baseType)
        {
            baseType = method.DeclaringType?.BaseType;
            return method.IsVirtual && !method.IsAbstract;
        }
    }

    private bool TryWalkInheritance(
        MethodDefinition method,
        ITypeDefOrRef? baseType,
        [NotNullWhen(true)] out MethodDefinition? baseMethod)
    {
        baseMethod = null;
        while (baseType is not null)
        {
            var resolved = baseType.Resolve();
            if (resolved is null)
            {
                _logger.Warning("Failed to resolve type {Type}", baseType);
                return false;
            }

            if (TryFindMethod(method, baseType.ToTypeSignature(), resolved, out baseMethod))
                return true;

            baseType = resolved.BaseType;
        }

        return false;
    }

    private static bool TryFindMethod(
        MethodDefinition needle,
        TypeSignature genericProvider,
        TypeDefinition type,
        [NotNullWhen(true)] out MethodDefinition? result)
    {
        result = null;
        foreach (var method in type.Methods)
        {
            if (!Overrides(needle, method, genericProvider))
                continue;

            result = method;
            return true;
        }

        return false;

        static bool Overrides(MethodDefinition baseMethod, MethodDefinition method, TypeSignature genericProvider)
        {
            if (method.IsStatic || method.Name != baseMethod.Name)
                return false;

            return Comparer.Equals(baseMethod.Signature!, method.Signature!, genericProvider);
        }
    }
}
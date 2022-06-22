using System.Diagnostics.CodeAnalysis;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Doktr.Lifters.Common.DependencyGraph;
using Serilog;

namespace Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;

public class ConstructorAnalyzer : IDependencyGraphAnalyzer<IMemberDefinition>
{
    private readonly ILogger _logger;

    public ConstructorAnalyzer(ILogger logger)
    {
        _logger = logger;
    }

    public void AnalyzeNode(DependencyNode<IMemberDefinition> node)
    {
        if (node.Value is not MethodDefinition method)
            return;
        if (!IsInstanceConstructor(method))
            return;
        if (method.CilMethodBody is not { } body)
            return;

        if (!TryGetOtherConstructorCall(body, out var otherCtor))
        {
            _logger.Warning("Failed to resolve the base/other constructor call of {Ctor}", method);
            return;
        }

        _logger.Verbose("The base/other constructor call of {Ctor} is {BaseCtor}", method, otherCtor);
        node.ParentGraph.AddDependency(method, otherCtor, DependencyEdgeKind.OtherConstructor);
    }

    private static bool TryGetOtherConstructorCall(CilMethodBody body, [NotNullWhen(true)] out MethodDefinition? call)
    {
        call = null;
        foreach (var instruction in body.Instructions)
        {
            if (!IsCall(instruction, out var target))
                continue;

            var resolved = target.Resolve();
            if (resolved is null || !IsInstanceConstructor(resolved))
                continue;

            call = resolved;
            return true;
        }

        return false;
    }

    private static bool IsInstanceConstructor(MethodDefinition method)
    {
        return method.IsConstructor && !method.IsStatic;
    }

    private static bool IsCall(CilInstruction instruction, [NotNullWhen(true)] out IMethodDefOrRef? target)
    {
        target = null;
        if (instruction.OpCode.Code != CilCode.Call)
            return false;
        if (instruction.Operand is not IMethodDefOrRef method)
            return false;

        target = method;
        return true;
    }
}
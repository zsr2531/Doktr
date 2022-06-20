using System.Diagnostics.CodeAnalysis;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Doktr.Lifters.Common.DependencyGraph;
using Serilog;

namespace Doktr.Lifters.AsmResolver.DependencyGraph.Analyzers;

public class BaseConstructorAnalyzer : IDependencyGraphAnalyzer<IMemberDefinition>
{
    private readonly ILogger _logger;

    public BaseConstructorAnalyzer(ILogger logger)
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

        var candidate = AnalyzeBody(body);
        if (candidate is null)
        {
            _logger.Warning("Failed to resolve the base constructor call of {Ctor}", method);
            return;
        }

        _logger.Verbose("The base constructor call of {Ctor} is {BaseCtor}", method, candidate);
        node.ParentGraph.AddDependency(method, candidate, DependencyEdgeKind.OtherConstructor);
    }

    private static MethodDefinition? AnalyzeBody(CilMethodBody body)
    {
        foreach (var instruction in body.Instructions)
        {
            if (!IsCall(instruction, out var target))
                continue;

            var resolved = target.Resolve();
            if (resolved is not null && IsInstanceConstructor(resolved))
                return resolved;
        }

        return null;
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
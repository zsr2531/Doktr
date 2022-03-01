using System.Collections.Generic;
using System.Linq;
using AsmResolver.DotNet;
using Doktr.Dependencies;
using Serilog;

namespace Doktr.Services.GraphTransformers;

public class VirtualMethodTransformer : IDependencyGraphTransformer
{
    private readonly IMetadataResolutionService _resolution;
    private readonly IGenericInstantiationService _generic;
    private readonly ILogger _logger;

    public VirtualMethodTransformer(
        IMetadataResolutionService resolution,
        IGenericInstantiationService generic,
        ILogger logger)
    {
        _resolution = resolution;
        _generic = generic;
        _logger = logger;
    }

    public string Name => nameof(VirtualMethodTransformer);

    public void VisitNode(DependencyNode node, GraphBuilderContext context)
    {
        if (node.MetadataMember is not MethodDefinition
            {
                IsVirtual: true, IsAbstract: false, DeclaringType: { BaseType: { } baseType }
            } method)
            return;

        var queue = new Queue<ITypeDefOrRef>();
        queue.Enqueue(baseType);

        while (queue.Count > 0)
        {
            var type = queue.Dequeue();
            var baseMethod = VisitType(type, queue, method);
            if (baseMethod is null)
                continue;

            context.AddDependency(method, baseMethod);
            _logger.Debug("'{Method}' overrides '{Base}'", method, baseMethod);
            return;
        }
    }

    private MethodDefinition? VisitType(ITypeDefOrRef type, Queue<ITypeDefOrRef> queue, MethodDefinition needle)
    {
        var typeSignature = type.ToTypeSignature();
        var resolved = _resolution.ResolveType(type);
        if (resolved == null)
            return null;

        foreach (var candidate in resolved.Methods.Where(m => m.IsVirtual && m.Name == needle.Name))
        {
            if (_generic.Equals(needle.Signature!, candidate.Signature!, typeSignature))
                return candidate;
        }

        if (_resolution.ResolveType(resolved.BaseType) is { })
            queue.Enqueue(resolved.BaseType!);

        return null;
    }
}
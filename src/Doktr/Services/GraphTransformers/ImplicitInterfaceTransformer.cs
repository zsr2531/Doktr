using System.Linq;
using AsmResolver.DotNet;
using Doktr.Dependencies;
using Serilog;

namespace Doktr.Services.GraphTransformers;

public class ImplicitInterfaceTransformer : IDependencyGraphTransformer
{
    private readonly IMetadataResolutionService _resolution;
    private readonly IGenericInstantiationService _generic;
    private readonly ILogger _logger;

    public ImplicitInterfaceTransformer(
        IMetadataResolutionService resolution,
        IGenericInstantiationService generic,
        ILogger logger)
    {
        _resolution = resolution;
        _generic = generic;
        _logger = logger;
    }

    public string Name => nameof(ImplicitInterfaceTransformer);

    public void VisitNode(DependencyNode node, GraphBuilderContext context)
    {
        if (node.MetadataMember is not TypeDefinition { IsClass: true, Interfaces: { Count: >0 } interfaces } type)
            return;

        foreach (var inf in interfaces.Select(impl => impl.Interface))
            ProcessInterface(type, inf, context);
    }

    private void ProcessInterface(TypeDefinition type, ITypeDefOrRef inf, GraphBuilderContext context)
    {
        _logger.Debug("Processing interface '{Interface}'", inf);
            
        var typeSignature = inf.ToTypeSignature();
        var resolved = _resolution.ResolveType(inf);
        if (resolved is null)
            return;
            
        foreach (var needle in resolved.Methods.Where(m => !m.IsStatic))
        {
            var candidates = type.Methods.Where(m => !m.IsStatic && m.Name == needle.Name);
            foreach (var candidate in candidates)
            {
                if (!_generic.Equals(needle.Signature, candidate.Signature, typeSignature))
                    continue;

                _logger.Debug("'{Method}' implicitly implements {BaseMethod}", candidate, needle);
                context.AddDependency(candidate, needle);
                break;
            }
        }
    }
}
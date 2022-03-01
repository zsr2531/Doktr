using AsmResolver.DotNet;
using Doktr.Dependencies;
using Serilog;

namespace Doktr.Services.GraphTransformers;

public class ExplicitInterfaceTransformer : IDependencyGraphTransformer
{
    private readonly IMetadataResolutionService _resolution;
    private readonly ILogger _logger;

    public ExplicitInterfaceTransformer(IMetadataResolutionService resolution, ILogger logger)
    {
        _resolution = resolution;
        _logger = logger;
    }

    public string Name => nameof(ExplicitInterfaceTransformer);

    public void VisitNode(DependencyNode node, GraphBuilderContext context)
    {
        if (node.MetadataMember is not TypeDefinition { MethodImplementations: { Count: > 0 } impls })
            return;

        foreach (var impl in impls)
            ProcessExplicitImplementation(impl, context);
    }

    private void ProcessExplicitImplementation(MethodImplementation impl, GraphBuilderContext context)
    {
        _logger.Verbose("Processing explicit implementation '{Impl}'", impl);

        var method = _resolution.ResolveMethod(impl.Body);
        var declaration = _resolution.ResolveMethod(impl.Declaration);
        if (method is null || declaration is null)
        {
            _logger.Warning("Failed to process explicit implementation '{Impl}'", impl);
            return;
        }

        _logger.Debug("'{Method}' explicitly implements '{Declaration}'", method, declaration);
        context.AddDependency(method, declaration);
    }
}
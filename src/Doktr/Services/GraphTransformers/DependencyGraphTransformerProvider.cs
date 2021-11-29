using System.Collections.Immutable;
using Serilog;

namespace Doktr.Services.GraphTransformers;

public class DependencyGraphTransformerProvider : IDependencyGraphTransformerProvider
{
    public DependencyGraphTransformerProvider(
        IMetadataResolutionService resolution,
        IGenericInstantiationService generic,
        ILogger logger)
    {
        var builder = ImmutableArray.CreateBuilder<IDependencyGraphTransformer>();
            
        builder.Add(new ExplicitInterfaceTransformer(resolution, logger));
        builder.Add(new ConstructorTransformer(resolution, logger));
        builder.Add(new VirtualMethodTransformer(resolution, generic, logger));
        builder.Add(new ImplicitInterfaceTransformer(resolution, generic, logger));

        Transformers = builder.ToImmutable();
    }
        
    public ImmutableArray<IDependencyGraphTransformer> Transformers
    {
        get;
    }
}
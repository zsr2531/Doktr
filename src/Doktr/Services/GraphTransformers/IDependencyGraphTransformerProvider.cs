using System.Collections.Immutable;

namespace Doktr.Services.GraphTransformers
{
    public interface IDependencyGraphTransformerProvider
    {
        ImmutableArray<IDependencyGraphTransformer> Transformers
        {
            get;
        }
    }
}
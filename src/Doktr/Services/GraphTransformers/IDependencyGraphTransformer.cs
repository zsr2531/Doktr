using Doktr.Dependencies;

namespace Doktr.Services.GraphTransformers;

public interface IDependencyGraphTransformer
{
    string Name
    {
        get;
    }

    void VisitNode(DependencyNode node, GraphBuilderContext context);
}
using Doktr.Lifters.Common.DependencyGraph.Collections;

namespace Doktr.Lifters.Common.DependencyGraph;

public class DependencyGraph<T>
    where T : notnull
{
    public DependencyNodeSet<T> Nodes { get; } = new();
    public DependencyNodeMap<T> NodeMap { get; } = new();

    public DependencyNode<T> AddNode(DependencyNode<T> node)
    {
        Nodes.Add(node);
        return node;
    }

    public DependencyNode<T> AddNode(T value) => AddNode(GetOrCreateNodeFor(value));

    public bool AddEdge(DependencyEdge<T> edge) =>
        edge.From.AddDependency(edge.To) | edge.To.AddDependant(edge.From);

    public bool AddDependency(DependencyNode<T> from, DependencyNode<T> to, DependencyEdgeKind kind) =>
        AddEdge(new DependencyEdge<T>(from, to, kind));

    public bool AddDependency(T from, T to, DependencyEdgeKind kind) => AddDependency(AddNode(from), AddNode(to), kind);

    // This creates the node but DOES NOT add it to the graph!
    // For that use AddNode().
    private DependencyNode<T> GetOrCreateNodeFor(T value) => NodeMap.GetOrCreateNodeFor(this, value);
}
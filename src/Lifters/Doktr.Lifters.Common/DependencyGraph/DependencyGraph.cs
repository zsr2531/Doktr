using Doktr.Lifters.Common.DependencyGraph.Collections;

namespace Doktr.Lifters.Common.DependencyGraph;

public class DependencyGraph<T>
    where T : notnull
{
    public DependencyNodeSet<T> Nodes { get; } = new();
    public DependencyEdgeSet<T> Edges { get; } = new();
    public DependencyNodeMap<T> NodeMap { get; } = new();

    public bool AddNode(DependencyNode<T> node) => Nodes.Add(node);

    public bool AddNode(T value) => AddNode(GetOrCreateNodeFor(value));

    public bool AddEdge(DependencyEdge<T> edge) =>
        Edges.Add(edge) | edge.From.AddDependency(edge.To) | edge.To.AddDependant(edge.From);

    public bool AddDependency(DependencyNode<T> from, DependencyNode<T> to) => AddEdge(new DependencyEdge<T>(from, to));

    public bool AddDependency(T from, T to) => AddDependency(GetOrCreateNodeFor(from), GetOrCreateNodeFor(to));

    // This creates the node but DOES NOT add it to the graph!
    // For that use AddNode().
    private DependencyNode<T> GetOrCreateNodeFor(T value) => NodeMap.GetOrCreateNodeFor(value);
}
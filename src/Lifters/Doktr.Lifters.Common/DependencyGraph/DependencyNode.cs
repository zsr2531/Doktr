using Doktr.Lifters.Common.DependencyGraph.Collections;

namespace Doktr.Lifters.Common.DependencyGraph;

public class DependencyNode<T> : IEquatable<DependencyNode<T>>
    where T : notnull
{
    public DependencyNode(DependencyGraph<T> parentGraph, T value)
    {
        ParentGraph = parentGraph;
        Value = value;
    }

    public DependencyGraph<T> ParentGraph { get; }
    public DependencyEdgeSet<T> Edges { get; } = new();
    public T Value { get; }

    public bool AddEdge(DependencyEdge<T> edge) => Edges.Add(edge);

    public IEnumerable<DependencyEdge<T>> GetDependencies() => Edges.Where(e => e.From == this);

    public IEnumerable<DependencyEdge<T>> GetDependants() => Edges.Where(e => e.To == this);

    public bool Equals(DependencyNode<T>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        return Equals((DependencyNode<T>) obj);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString()!;

    public static bool operator ==(DependencyNode<T>? left, DependencyNode<T>? right) => Equals(left, right);

    public static bool operator !=(DependencyNode<T>? left, DependencyNode<T>? right) => !Equals(left, right);
}
namespace Doktr.Lifters.Common.DependencyGraph;

public readonly struct DependencyEdge<T> : IEquatable<DependencyEdge<T>>
    where T : notnull
{
    public DependencyEdge(DependencyNode<T> from, DependencyNode<T> to, DependencyEdgeKind kind)
    {
        From = from;
        To = to;
        Kind = kind;
    }

    public DependencyNode<T> From { get; }
    public DependencyNode<T> To { get; }
    public DependencyEdgeKind Kind { get; }

    public bool Equals(DependencyEdge<T> other) => From.Equals(other.From) && To.Equals(other.To) && Kind == other.Kind;

    public override bool Equals(object? obj) => obj is DependencyEdge<T> other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(From, To, Kind);

    public static bool operator ==(DependencyEdge<T> left, DependencyEdge<T> right) => left.Equals(right);

    public static bool operator !=(DependencyEdge<T> left, DependencyEdge<T> right) => !left.Equals(right);
}
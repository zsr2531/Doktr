using Doktr.Lifters.Common.DependencyGraph.Collections;

namespace Doktr.Lifters.Common.DependencyGraph;

public class DependencyNode<T> : IEquatable<DependencyNode<T>>
    where T : notnull
{
    public DependencyNode(T value)
    {
        Value = value;
    }

    public T Value { get; }
    public DependencyNodeSet<T> Dependencies { get; } = new();
    public DependencyNodeSet<T> Dependants { get; } = new();

    public bool AddDependency(DependencyNode<T> dependency) => Dependencies.Add(dependency);

    public bool AddDependant(DependencyNode<T> dependant) => Dependants.Add(dependant);

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

    public static bool operator ==(DependencyNode<T>? left, DependencyNode<T>? right) => Equals(left, right);

    public static bool operator !=(DependencyNode<T>? left, DependencyNode<T>? right) => !Equals(left, right);
}
namespace Doktr.Lifters.Common.DependencyGraph.Collections;

public class DependencyNodeMap<T> : Dictionary<T, DependencyNode<T>>
    where T : notnull
{
    internal DependencyNode<T> GetOrCreateNodeFor(T value)
    {
        if (TryGetValue(value, out var node))
            return node;

        node = new DependencyNode<T>(value);
        Add(value, node);
        return node;
    }
}
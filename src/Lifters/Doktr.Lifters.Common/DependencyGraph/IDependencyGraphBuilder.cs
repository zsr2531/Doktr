namespace Doktr.Lifters.Common.DependencyGraph;

public interface IDependencyGraphBuilder<T>
    where T : notnull
{
    DependencyGraph<T> BuildDependencyGraph();
}
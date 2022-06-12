namespace Doktr.Lifters.Common.DependencyGraph;

public interface IDependencyGraphAnalyzer<T>
    where T : notnull
{
    void AnalyzeNode(DependencyNode<T> node);
}
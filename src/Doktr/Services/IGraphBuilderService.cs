using Doktr.Dependencies;

namespace Doktr.Services;

public interface IGraphBuilderService
{
    DependencyGraph BuildGraph();
}
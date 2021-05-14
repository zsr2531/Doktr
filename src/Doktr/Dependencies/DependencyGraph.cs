using System.Collections.Immutable;
using AsmResolver.DotNet;

namespace Doktr.Dependencies
{
    public class DependencyGraph
    {
        public DependencyGraph(
            ImmutableDictionary<IFullNameProvider, DependencyNode> mapping,
            ImmutableArray<DependencyNode> roots)
        {
            Mapping = mapping;
            Roots = roots;
        }

        public ImmutableDictionary<IFullNameProvider, DependencyNode> Mapping
        {
            get;
        }
        
        public ImmutableArray<DependencyNode> Roots
        {
            get;
        }
    }
}
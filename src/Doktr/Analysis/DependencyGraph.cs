using System.Collections.Generic;
using AsmResolver.DotNet;

namespace Doktr.Analysis
{
    public class DependencyGraph
    {
        public DependencyGraph(IReadOnlyDictionary<IFullNameProvider, DependencyNode> nodes, DependencyNode root)
        {
            Nodes = nodes;
            Root = root;
        }

        public IReadOnlyDictionary<IFullNameProvider, DependencyNode> Nodes
        {
            get;
        }
        
        public DependencyNode Root
        {
            get;
        }
    }
}
using System.Collections.Generic;
using AsmResolver.DotNet;

namespace Doktr.Analysis
{
    public class DependencyGraph
    {
        public DependencyGraph(IReadOnlyDictionary<INameProvider, DependencyNodeBase> nodes, DependencyNode root)
        {
            Nodes = nodes;
            Root = root;
        }

        public IReadOnlyDictionary<INameProvider, DependencyNodeBase> Nodes
        {
            get;
        }
        
        public DependencyNode Root
        {
            get;
        }
    }
}
using System.Collections.Immutable;
using AsmResolver.DotNet;
using Doktr.Dependencies;

namespace Doktr.Services
{
    public class GraphBuilderContext
    {
        private readonly ImmutableDictionary<IFullNameProvider, DependencyNode> _mapping;

        public GraphBuilderContext(ImmutableDictionary<IFullNameProvider, DependencyNode> mapping)
        {
            _mapping = mapping;
        }

        public DependencyNode GetOrCreateNode(IFullNameProvider metadataMember, DependencyNode? parent = null)
        {
            if (_mapping.TryGetValue(metadataMember, out var node))
                return node;

            node = new DependencyNode(metadataMember, parent);
            return node;
        }
    }
}
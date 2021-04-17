using System.Collections.Generic;
using AsmResolver.DotNet;

namespace Doktr.Analysis.Transformations
{
    public class TransformationContext
    {
        public TransformationContext(IDictionary<IFullNameProvider, DependencyNode> mapping)
        {
            Mapping = mapping;
        }

        public IDictionary<IFullNameProvider, DependencyNode> Mapping
        {
            get;
        }

        public DependencyNode GetOrCreateNode(IFullNameProvider member)
        {
            if (Mapping.TryGetValue(member, out var value))
                return value;
            
            var node = new DependencyNode(member);
            Mapping.Add(member, node);

            return node;
        }
    }
}
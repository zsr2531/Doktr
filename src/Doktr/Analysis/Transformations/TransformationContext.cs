using System.Collections.Generic;
using AsmResolver.DotNet;

namespace Doktr.Analysis.Transformations
{
    public class TransformationContext
    {
        public TransformationContext(IDictionary<INameProvider, DependencyNodeBase> mapping)
        {
            Mapping = mapping;
        }

        public IDictionary<INameProvider, DependencyNodeBase> Mapping
        {
            get;
        }

        public DependencyNodeBase GetOrCreateNode(INameProvider member)
        {
            if (Mapping.TryGetValue(member, out var value))
                return value;
            
            var node = new ExternalDependencyNode(member);
            Mapping.Add(member, node);

            return node;
        }
    }
}
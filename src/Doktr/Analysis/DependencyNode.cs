using System.Collections.Generic;
using System.Diagnostics;
using AsmResolver.DotNet;
using Doktr.Collections;

namespace Doktr.Analysis
{
    [DebuggerDisplay("{" + nameof(MetadataMember) + ",nq}")]
    public class DependencyNode : DependencyNodeBase, IHasOwner<DependencyNode>
    {
        public DependencyNode(INameProvider metadataMember)
            : base(metadataMember)
        {
            Children = new OwnerCollection<DependencyNode>(this);
        }

        public DependencyNode Owner
        {
            get;
            set;
        }

        public IList<DependencyNode> Children
        {
            get;
        }
    }
}
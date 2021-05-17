using System.Collections.Generic;
using System.Diagnostics;
using AsmResolver.DotNet;

namespace Doktr.Dependencies
{
    [DebuggerDisplay("{MetadataMember.FullName}")]
    public class DependencyNode
    {
        public DependencyNode(IFullNameProvider metadataMember, DependencyNode? parent = null)
        {
            MetadataMember = metadataMember;
            Parent = parent;
            Children = new OwnerCollection(this);
            Dependencies = new DependencySet(this);
            Dependants = new DependantSet(this);
            
            parent?.Children.Add(this);
        }

        public IFullNameProvider MetadataMember
        {
            get;
        }
        
        public DependencyNode? Parent
        {
            get;
            internal set;
        }
        
        public ICollection<DependencyNode> Children
        {
            get;
        }

        public ISet<DependencyNode> Dependencies
        {
            get;
        }

        public ISet<DependencyNode> Dependants
        {
            get;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AsmResolver.DotNet;
using Doktr.Collections;

namespace Doktr.Analysis
{
    [DebuggerDisplay("{" + nameof(MetadataMember) + ",nq}")]
    public class DependencyNode : IHasOwner<DependencyNode>
    {
        public DependencyNode(IFullNameProvider metadataMember)
        {
            MetadataMember = metadataMember ?? throw new ArgumentNullException(nameof(metadataMember));
            Dependencies = new DependencyCollection(this);
            Dependants = new DependantCollection(this);
            Children = new OwnerCollection<DependencyNode>(this);
        }

        public IFullNameProvider MetadataMember
        {
            get;
        }
        
        public ICollection<DependencyNode> Dependencies
        {
            get;
        }

        public ICollection<DependencyNode> Dependants
        {
            get;
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
using System;
using System.Collections.Generic;
using AsmResolver.DotNet;
using Doktr.Collections;

namespace Doktr.Analysis
{
    public abstract class DependencyNodeBase
    {
        protected DependencyNodeBase(INameProvider metadataMember)
        {
            MetadataMember = metadataMember ?? throw new ArgumentNullException(nameof(metadataMember));
            Dependencies = new DependencyCollection(this);
            Dependants = new DependantCollection(this);
        }

        public INameProvider MetadataMember
        {
            get;
        }
        
        public ICollection<DependencyNodeBase> Dependencies
        {
            get;
        }

        public ICollection<DependencyNodeBase> Dependants
        {
            get;
        }
    }
}
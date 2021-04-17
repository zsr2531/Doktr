using System;
using System.Diagnostics;
using Doktr.Analysis;

namespace Doktr.Collections
{
    [DebuggerDisplay("Count = {" + nameof(Count) + ",nq}")]
    public class DependencyCollection : NeoCollection<DependencyNode>
    {
        private readonly DependencyNode _parent;
        private bool _removing;

        public DependencyCollection(DependencyNode parent)
        {
            _parent = parent;
        }
        
        protected override void RemoveItem(int index, DependencyNode item)
        {
            if (_removing)
                return;

            _removing = true;
            base.RemoveItem(index, item);
            item.Dependants.Remove(_parent);
            _removing = false;
        }

        protected override void InsertItem(int index, DependencyNode item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (Contains(item))
                return;
            
            base.InsertItem(index, item);
            item.Dependants.Add(_parent);
        }
    }
}
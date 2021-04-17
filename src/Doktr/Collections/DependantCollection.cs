using System;
using System.Diagnostics;
using Doktr.Analysis;

namespace Doktr.Collections
{
    [DebuggerDisplay("Count = {" + nameof(Count) + ",nq}")]
    public class DependantCollection : NeoCollection<DependencyNode>
    {
        private readonly DependencyNode _parent;
        private bool _removing;

        public DependantCollection(DependencyNode parent)
        {
            _parent = parent;
        }

        protected override void RemoveItem(int index, DependencyNode item)
        {
            if (_removing)
                return;
            
            base.RemoveItem(index);
            item.Dependencies.Remove(_parent);
            _removing = false;
        }

        protected override void InsertItem(int index, DependencyNode item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (Contains(item))
                return;

            base.InsertItem(index, item);
            item.Dependencies.Add(_parent);
        }
    }
}
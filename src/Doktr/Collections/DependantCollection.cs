using System;
using System.Diagnostics;
using Doktr.Analysis;

namespace Doktr.Collections
{
    [DebuggerDisplay("Count = {" + nameof(Count) + ",nq}")]
    public class DependantCollection : NeoCollection<DependencyNodeBase>
    {
        private readonly DependencyNodeBase _parent;
        private bool _removing;

        public DependantCollection(DependencyNodeBase parent)
        {
            _parent = parent;
        }

        protected override void RemoveItem(int index, DependencyNodeBase item)
        {
            if (_removing)
                return;
            
            base.RemoveItem(index);
            item.Dependencies.Remove(_parent);
            _removing = false;
        }

        protected override void InsertItem(int index, DependencyNodeBase item)
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
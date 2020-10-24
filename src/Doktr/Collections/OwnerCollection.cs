using System;
using System.Diagnostics;

namespace Doktr.Collections
{
    [DebuggerDisplay("Count = {" + nameof(Count) + ",nq}")]
    public class OwnerCollection<T> : NeoCollection<T>
        where T : class, IHasOwner<T>
    {
        private readonly T _instance;

        public OwnerCollection(T instance)
        {
            _instance = instance;
        }

        protected override void RemoveItem(int index, T item)
        {
            item.Owner = null;
            base.RemoveItem(index, item);
        }

        protected override void InsertItem(int index, T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            
            item.Owner = _instance;
            base.InsertItem(index, item);
        }
    }
}
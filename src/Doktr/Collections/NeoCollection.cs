using System.Collections.ObjectModel;

namespace Doktr.Collections
{
    public abstract class NeoCollection<T> : Collection<T>
    {
        protected virtual void RemoveItem(int index, T item) { }
        
        protected sealed override void ClearItems()
        {
            int count = Count;
            for (int i = 0; i < count; i++)
                RemoveItem(0);
        }

        protected sealed override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }

        protected sealed override void SetItem(int index, T item)
        {
            var old = this[index];
            RemoveItem(index, old);
            InsertItem(index, item);
        }
    }
}
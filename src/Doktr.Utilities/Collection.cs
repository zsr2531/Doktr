namespace Doktr.Utilities
{
    public class Collection<T> : System.Collections.ObjectModel.Collection<T>
    {
        protected virtual void AddItem(int index, T item)
        {
        }

        protected virtual void RemoveItem(int index, T item)
        {
        }

        protected sealed override void ClearItems()
        {
            int length = Count;
            for (int i = 0; i < length; i++)
                RemoveItem(0);
        }

        protected sealed override void InsertItem(int index, T item)
        {
            AddItem(index, item);
            base.InsertItem(index, item);
        }

        protected sealed override void RemoveItem(int index)
        {
            var item = this[index];
            RemoveItem(index, item);
            
            base.RemoveItem(index);
        }

        protected sealed override void SetItem(int index, T item)
        {
            AddItem(index, item);
            base.SetItem(index, item);
        }
    }
}
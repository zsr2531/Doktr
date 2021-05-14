using System.Collections;
using System.Collections.Generic;

namespace Doktr.Dependencies
{
    public class DependencySet : ISet<DependencyNode>
    {
        private readonly HashSet<DependencyNode> _set = new();
        private readonly DependencyNode _owner;
        private bool _isBusy;

        public DependencySet(DependencyNode owner)
        {
            _owner = owner;
        }

        public int Count => _set.Count;

        public bool IsReadOnly => false;

        public bool Add(DependencyNode item)
        {
            if (_isBusy)
                return false;

            _isBusy = true;
            item.Dependants.Add(_owner);
            _isBusy = false;
            
            return _set.Add(item);
        }

        void ICollection<DependencyNode>.Add(DependencyNode item) => Add(item);

        public void Clear()
        {
            if (_isBusy)
                return;

            _isBusy = true;
            foreach (var item in _set)
                item.Dependants.Remove(_owner);
            _isBusy = false;
            
            _set.Clear();
        }

        public bool Contains(DependencyNode item) => _set.Contains(item);

        public void CopyTo(DependencyNode[] array, int arrayIndex) => _set.CopyTo(array, arrayIndex);

        public void ExceptWith(IEnumerable<DependencyNode> other) => _set.ExceptWith(other);

        public void IntersectWith(IEnumerable<DependencyNode> other) => _set.IntersectWith(other);

        public bool IsProperSubsetOf(IEnumerable<DependencyNode> other) => _set.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<DependencyNode> other) => _set.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<DependencyNode> other) => _set.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<DependencyNode> other) => _set.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<DependencyNode> other) => _set.Overlaps(other);

        public bool Remove(DependencyNode item)
        {
            if (_isBusy)
                return false;

            _isBusy = true;
            item.Dependants.Remove(_owner);
            _isBusy = false;
            
            return _set.Remove(item);
        }

        public bool SetEquals(IEnumerable<DependencyNode> other) => _set.SetEquals(other);

        public void SymmetricExceptWith(IEnumerable<DependencyNode> other) => _set.SymmetricExceptWith(other);

        public void UnionWith(IEnumerable<DependencyNode> other) => _set.UnionWith(other);

        public IEnumerator<DependencyNode> GetEnumerator() => _set.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _set.GetEnumerator();
    }
}
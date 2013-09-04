using System;
using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.DataStructures.Collections
{
    public class KeyedSet<K, T> : IEnumerable<T>
    {
        private readonly Func<T, K> _getKey;
        private readonly Dictionary<K, T> _set;

        public KeyedSet(Func<T, K> getKey, IEnumerable<T> items)
        {
            _getKey = getKey; 
            _set = new Dictionary<K, T>();
            Add(items);
        }

        public void Add(T item)
        {
            _set.Add(_getKey(item), item);
        }

        public void Add(IEnumerable<T> items)
        {
            items.ForEach(Add);
        }

        public T this[K key]
        {
            get { return _set[key]; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _set.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

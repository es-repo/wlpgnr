using System;
using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.DataStructures.Collections
{
    public class KeyedSet<K, T> : IEnumerable<T>
    {
        private readonly Dictionary<K, T> _set;

        public KeyedSet(Func<T, K> getKey, IEnumerable<T> items)
        {
            _set = new Dictionary<K, T>();
            foreach (T i in items)
            {
                _set.Add(getKey(i), i);
            }
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

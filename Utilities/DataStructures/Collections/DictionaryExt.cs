using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Collections
{
    public class DictionaryExt<K, T> : Dictionary<K, T>
    {
        private readonly Func<T, K> _getKey;

        public DictionaryExt(IEnumerable<KeyValuePair<K, T>> items)
        {
            items.ForEach(i => Add(i.Key, i.Value));
        }

        public DictionaryExt(Func<T, K> getKey)
            : this(getKey, Enumerable.Empty<T>())
        {
        }

        public DictionaryExt(Func<T, K> getKey, IEnumerable<T> items)
        {
            _getKey = getKey; 
            Add(items);
        }

        public void Add(T item)
        {
            Add(_getKey(item), item);
        }

        public void Add(IEnumerable<T> items)
        {
            items.ForEach(Add);
        }
    }
}

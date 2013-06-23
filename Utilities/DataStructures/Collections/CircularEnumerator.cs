using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.DataStructures.Collections
{
    public class CircularEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _elements;
        private int _index;

        public CircularEnumerator(IEnumerable<T> elements)
        {
            _elements = elements.ToArray();
            Reset();
        }

        public T Current
        {
            get { return _elements[_index]; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            _index++;
            if (_index == _elements.Length)
            {
                _index = 0;
            }
            return true;
        }

        public void Reset()
        {
            _index = -1;
        }

        public void Dispose()
        {
        }
    }

}

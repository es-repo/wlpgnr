using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.DataStructures.Collections
{    
    public class CircularEnumeration<T> : IEnumerable<T>
    {
        private readonly CircularEnumerator<T> _enumerator;

        public CircularEnumeration(IEnumerable<T> elements)
        {
            _enumerator = new CircularEnumerator<T>(elements);
        }

        public T Next
        {
            get
            {
                _enumerator.MoveNext();
                return _enumerator.Current;
            }
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerator; 
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerator;
        }

        #endregion
    }
}

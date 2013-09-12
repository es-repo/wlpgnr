using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities
{
    public class EnumerableNext<T> : IEnumerable<T>
    {
        protected IEnumerator<T> Enumerator { get; set; }

        public EnumerableNext(IEnumerable<T> enumerable)
        {
            Enumerator = enumerable.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual T Next()
        {
            Enumerator.MoveNext();
            return Enumerator.Current;
        }
    }
}

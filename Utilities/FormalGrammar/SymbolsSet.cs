using System.Collections;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class SymbolsSet<T> : IEnumerable<Symbol<T>>
    {
        private readonly Dictionary<string, Symbol<T>> _set;

        public SymbolsSet(IEnumerable<Symbol<T>> symbols)
        {
            _set = new Dictionary<string, Symbol<T>>();
            foreach (Symbol<T> s in symbols)
            {
                _set.Add(s.Name, s);
            }
        }

        public Symbol<T> this[string name]
        {
            get { return _set[name]; }
        }

        public IEnumerator<Symbol<T>> GetEnumerator()
        {
            return _set.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

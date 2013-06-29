using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{        
    public class Rule<T>
    {
        private readonly Func<IEnumerable<Symbol<T>>> _apply;

        public string Name { get; private set; }

        public Symbol<T> From { get; private set; }

        protected Rule(Symbol<T> from)
            : this("", from, new Symbol<T>[] { })
        {
        }

        protected Rule(string name, Symbol<T> from)
            : this(name, from, new Symbol<T>[]{})
        {
        }

        public Rule(Symbol<T> from, Func<IEnumerable<Symbol<T>>> apply)
            : this("", from, apply)
        {
        }

        public Rule(string name, Symbol<T> from, Func<IEnumerable<Symbol<T>>> apply)
        {
            Name = name;
            From = from;
            _apply = apply;
        }

        public Rule(Symbol<T> from, IEnumerable<Symbol<T>> to)
            : this("", from, to)
        {
        }

        public Rule(string name, Symbol<T> from, IEnumerable<Symbol<T>> to)
            : this(name, from, () => to)
        {
        }

        public virtual IEnumerable<Symbol<T>> Apply()
        {
            return _apply();
        }
    }
}

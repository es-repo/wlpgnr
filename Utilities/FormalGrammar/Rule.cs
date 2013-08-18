using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{        
    public class Rule<T>
    {
        private readonly Func<IEnumerable<Symbol<T>>> _apply;

        public string Name { get; private set; }

        public Symbol<T> From { get; private set; }

        protected Rule(Symbol<T> from, string name)
            : this(from, new Symbol<T>[] { }, name)
        {
        }

        public Rule(Symbol<T> from, Func<IEnumerable<Symbol<T>>> apply, string name = "")
        {
            Name = name;
            From = from;
            _apply = apply;
        }

        public Rule(Symbol<T> from, IEnumerable<Symbol<T>> to, string name = "")
            : this(from, () => to, name)
        {
        }

        public Rule(IEnumerable<Symbol<T>> to)
            : this(null, to)
        {
        }

        public virtual IEnumerable<Symbol<T>> Apply()
        {
            return _apply();
        }
    }
}

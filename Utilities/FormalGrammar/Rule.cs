using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{        
    public class Rule<T>
    {
        private readonly Func<IEnumerable<Symbol<T>>> _produceFunc;

        public Symbol<T> From { get; private set; }

        protected Rule(Symbol<T> from)
            : this(from, new Symbol<T>[] { })
        {
        }

        public Rule(Symbol<T> from, Func<IEnumerable<Symbol<T>>> produceFunc)
        {
            From = from;
            _produceFunc = produceFunc;
        }

        public Rule(Symbol<T> from, IEnumerable<Symbol<T>> to)
            : this(from, () => to)
        {
        }

        public Rule(IEnumerable<Symbol<T>> to)
            : this(null, to)
        {
        }

        public virtual IEnumerable<Symbol<T>> Produce()
        {
            return _produceFunc();
        }
    }
}

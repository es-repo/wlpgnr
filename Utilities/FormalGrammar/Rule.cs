using System;
using System.Collections.Generic;
using System.Linq; 

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

        public Rule(string from, Func<IEnumerable<Symbol<T>>> produceFunc)
            : this(new Symbol<T>(from), produceFunc)
        {
        }

        public Rule(Symbol<T> from, Func<IEnumerable<Symbol<T>>> produceFunc)
        {
            From = from;
            _produceFunc = produceFunc;
        }

        public Rule(string from, IEnumerable<T> toTerminalsOnly)
            : this(new Symbol<T>(from), toTerminalsOnly.Select(v => new Symbol<T>(v.ToString(), v)))
        {
        }

        public Rule(string from, IEnumerable<Symbol<T>> to)
            : this(new Symbol<T>(from), to)
        {
        }

        public Rule(Symbol<T> from, IEnumerable<Symbol<T>> to)
            : this(from, () => to)
        {
        }

        public Rule(IEnumerable<T> toTerminalsOnly)
            : this(toTerminalsOnly.Select(v => new Symbol<T>(v.ToString(), v)))
        {
        }

        public Rule(IEnumerable<Symbol<T>> to)
            : this((Symbol<T>)null, to)
        {
        }

        public virtual IEnumerable<Symbol<T>> Produce()
        {
            return _produceFunc();
        }
    }
}

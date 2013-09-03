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

// ReSharper disable UnusedParameter.Local
        public Rule(string from, int terminalsOnlyMarker, IEnumerable<T> toTerminalsOnly)
// ReSharper restore UnusedParameter.Local
            : this(new Symbol<T>(from), toTerminalsOnly.Select(v => new Symbol<T>(v.ToString(), v)))
        {
        }

        public Rule(string from, IEnumerable<string> toNonTerminalsOnly)
            : this(new Symbol<T>(from), toNonTerminalsOnly.Select(v => new Symbol<T>(v)))
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

// ReSharper disable UnusedParameter.Local
        public Rule(int terminalsOnlyMarker, IEnumerable<T> toTerminalsOnly)
// ReSharper restore UnusedParameter.Local
            : this(toTerminalsOnly.Select(v => new Symbol<T>(v.ToString(), v)))
        {
        }

        public Rule(IEnumerable<string> toNonTerminalsOnly)
            : this(toNonTerminalsOnly.Select(v => new Symbol<T>(v)))
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

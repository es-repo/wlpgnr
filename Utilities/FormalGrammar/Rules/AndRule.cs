using System.Linq;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar.Rules
{
    public class AndRule<T> : CompositeRule<T>
    {
        public AndRule(string from, params Rule<T>[] rules)
            : base(new Symbol<T>(from), rules)
        {
        }

        public AndRule(Symbol<T> from, params Rule<T>[] rules)
            : base(from, rules)
        {
        }

        public AndRule(params Rule<T>[] rules)
            : this((Symbol<T>)null, rules)
        {
        }

        public override IEnumerable<Symbol<T>> Produce()
        {
            return Rules.SelectMany(r => r.Produce());
        }
    }
}

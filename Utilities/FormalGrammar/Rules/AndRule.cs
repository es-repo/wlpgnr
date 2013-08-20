using System.Linq;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar.Rules
{
    public class AndRule<T> : CompositeRule<T>
    {
        public AndRule(Symbol<T> from, params Rule<T>[] rules)
            : base(from, rules)
        {
        }

        public AndRule(params Rule<T>[] rules)
            : this(null, rules)
        {
        }

        public override IEnumerable<Symbol<T>> Apply()
        {
            return Rules.SelectMany(r => r.Apply());
        }
    }
}

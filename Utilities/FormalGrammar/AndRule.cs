using System.Linq;  
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class AndRule<T> : CompositeRule<T>
    {
        public AndRule(Symbol<T> from, string name = "", params Rule<T>[] rules)
            : base(from, name, rules)
        {
        }

        public AndRule(params Rule<T>[] rules)
            : this(null, rules: rules)
        {
        }

        public override IEnumerable<Symbol<T>> Apply()
        {
            return Rules.SelectMany(r => r.Apply());
        }
    }
}

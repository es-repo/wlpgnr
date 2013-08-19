using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{        
    public class Rule<T>
    {
        private readonly Func<IEnumerable<Symbol<T>>> _apply;

        public Symbol<T> From { get; private set; }

        public Rule(Symbol<T> from)
            : this(from, new Symbol<T>[] { })
        {
        }

        public Rule(Symbol<T> from, Func<IEnumerable<Symbol<T>>> apply)
        {
            From = from;
            _apply = apply;
        }

        public Rule(Symbol<T> from, IEnumerable<Symbol<T>> to)
            : this(from, () => to)
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

        public static Rule<T> Or(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
        {
            return new OrRule<T>(createRuleSelector, rules);
        }

        public static Rule<T> Or(params Rule<T>[] rules)
        {
            return new OrRule<T>(null, null, rules);
        }

        public static Rule<T> Or(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Symbol<T>[] to)
        {
            return new OrRule<T>(createRuleSelector, to);
        }

        public static Rule<T> Or(params Symbol<T>[] to)
        {
            return new OrRule<T>(null, null, to);
        }

        public static Rule<T> And(params Rule<T>[] rules)
        {
            return new AndRule<T>(rules);
        }
    }
}

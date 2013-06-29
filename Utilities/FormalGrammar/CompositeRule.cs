using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class CompositeRule<T> : Rule<T>
    {
        private readonly RuleSelector<T> _ruleSelector;

        public CompositeRule(Symbol<T> from, IEnumerable<Rule<T>> rules, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector)
            : this("", from, rules, createRuleSelector)
        {
        }

        public CompositeRule(string name, Symbol<T> from, IEnumerable<Rule<T>> rules, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector)
            : base(name, from)
        {
            _ruleSelector = createRuleSelector(rules);
        }

        public override IEnumerable<Symbol<T>> Apply()
        {
            Rule<T> rule = _ruleSelector.Select();
            return rule.Apply();
        }
    }
}

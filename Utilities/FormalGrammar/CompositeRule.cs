using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class CompositeRule<T> : Rule<T>
    {
        private readonly RuleSelector<T> _ruleSelector;

        public CompositeRule(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : this(null, createRuleSelector, rules: rules)
        {
        }

        public CompositeRule(Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, string name = "", params Rule<T>[] rules)
            : base(from, name)
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

using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class CompositeRule<T> : Rule<T>
    {
        private readonly RuleSelector<T> _ruleSelector;

        public CompositeRule(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : this("", null, createRuleSelector, rules)
        {
        }

        public CompositeRule(Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : this("", from, createRuleSelector, rules)
        {
        }

        public CompositeRule(string name, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : this(name, null, createRuleSelector, rules)
        {            
        }

        public CompositeRule(string name, Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
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

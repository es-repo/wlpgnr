using System;
using System.Collections.Generic;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class OrRule<T> : CompositeRule<T>
    {
        private readonly RuleSelector<T> _ruleSelector;

        public OrRule(Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector = null, 
            string name = "", params Rule<T>[] rules)
            : base(from, name, rules)
        {
            if (createRuleSelector == null)
            {
                createRuleSelector = rs => new CircularRuleSelector<T>(rs);
            }

            _ruleSelector = createRuleSelector(rules);
        }


        public OrRule(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : this(null, createRuleSelector, rules: rules)
        {
        }

        public OrRule(params Rule<T>[] rules)
            : this(null, rules)
        {
        }

        public override IEnumerable<Symbol<T>> Apply()
        {
            Rule<T> rule = _ruleSelector.Select();
            return rule.Apply();
        }
    }
}

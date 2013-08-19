using System;
using System.Linq;  
using System.Collections.Generic;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class OrRule<T> : CompositeRule<T>
    {
        private readonly RuleSelector<T> _ruleSelector;

        public OrRule(Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : base(from, rules)
        {
            if (createRuleSelector == null)
            {
                createRuleSelector = rs => new CircularRuleSelector<T>(rs);
            }

            _ruleSelector = createRuleSelector(rules);
        }

        public OrRule(Symbol<T> from, params Rule<T>[] rules)
            : this(from, null, rules)
        {
        }

        public OrRule(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : this(null, createRuleSelector, rules)
        {
        }

        public OrRule(params Rule<T>[] rules)
            : this(null, null, rules)
        {
        }

        public OrRule(Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, IEnumerable<Symbol<T>> to)
            : this(from, createRuleSelector, to.Select(s => new Rule<T>(s)).ToArray())
        {
        }

        public OrRule(Symbol<T> from, IEnumerable<Symbol<T>> to)
            : this(from, null, to)
        {
        }

        public OrRule(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, IEnumerable<Symbol<T>> to)
            : this(null, createRuleSelector, to.Select(s => new Rule<T>(new []{ s })).ToArray())
        {
        }

        public OrRule(IEnumerable<Symbol<T>> to)
            : this(null, null, to)
        {
        }

        public override IEnumerable<Symbol<T>> Apply()
        {
            Rule<T> rule = _ruleSelector.Select();
            return rule.Apply();
        }
    }
}

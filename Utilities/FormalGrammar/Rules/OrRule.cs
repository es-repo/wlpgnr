using System;
using System.Linq;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar.Rules
{
    public class OrRule<T> : CompositeRule<T>
    {
        public RuleSelector<T> RuleSelector { get; private set; }

        public OrRule(Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : base(from, rules)
        {
            if (createRuleSelector == null)
            {
                createRuleSelector = rs => new RuleSelector<T>(rs);
            }

            RuleSelector = createRuleSelector(rules);
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
            : this(from, createRuleSelector, to.Select(s => new Rule<T>(new [] { s })).ToArray())
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

        public override IEnumerable<Symbol<T>> Produce()
        {
            Rule<T> rule = RuleSelector.Next();
            return rule.Produce();
        }
    }
}

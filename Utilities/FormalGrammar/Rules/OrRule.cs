using System;
using System.Linq;
using System.Collections.Generic;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;

namespace WallpaperGenerator.Utilities.FormalGrammar.Rules
{
    public class OrRule<T> : CompositeRule<T>
    {
        private readonly RuleSelector<T> _ruleSelector;

        public OrRule(string from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : this(new Symbol<T>(from), createRuleSelector, rules)
        {}

        public OrRule(Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : base(from, rules)
        {
            if (createRuleSelector == null)
            {
                createRuleSelector = rs => new CircularRuleSelector<T>(rs);
            }

            _ruleSelector = createRuleSelector(rules);
        }

        public OrRule(string from, params Rule<T>[] rules)
            : this(new Symbol<T>(from), rules)
        {
        }

        public OrRule(Symbol<T> from, params Rule<T>[] rules)
            : this(from, null, rules)
        {
        }

        public OrRule(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, params Rule<T>[] rules)
            : this((Symbol<T>)null, createRuleSelector, rules)
        {
        }

        public OrRule(params Rule<T>[] rules)
            : this((Symbol<T>)null, null, rules)
        {
        }

        public OrRule(string from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, IEnumerable<Symbol<T>> to)
            : this(new Symbol<T>(from), createRuleSelector, to)
        {
        }

        public OrRule(Symbol<T> from, Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, IEnumerable<Symbol<T>> to)
            : this(from, createRuleSelector, to.Select(s => new Rule<T>(new [] { s })).ToArray())
        {
        }

        public OrRule(string from, IEnumerable<T> toTerminalsOnly)
            : this(from, toTerminalsOnly.Select(v => new Symbol<T>(v.ToString(), v)))
        {
        }

        public OrRule(string from, IEnumerable<Symbol<T>> to)
            : this(new Symbol<T>(from), to)
        {
        }

        public OrRule(Symbol<T> from, IEnumerable<Symbol<T>> to)
            : this(from, null, to)
        {
        }

        public OrRule(Func<IEnumerable<Rule<T>>, RuleSelector<T>> createRuleSelector, IEnumerable<Symbol<T>> to)
            : this((Symbol<T>)null, createRuleSelector, to.Select(s => new Rule<T>(new []{ s })).ToArray())
        {
        }

        public OrRule(IEnumerable<T> toTerminalsOnly)
            : this(toTerminalsOnly.Select(v => new Symbol<T>(v.ToString(), v)))
        {
        }

        public OrRule(IEnumerable<Symbol<T>> to)
            : this((Symbol<T>)null, null, to)
        {
        }

        public override IEnumerable<Symbol<T>> Produce()
        {
            Rule<T> rule = _ruleSelector.Next();
            return rule.Produce();
        }
    }
}

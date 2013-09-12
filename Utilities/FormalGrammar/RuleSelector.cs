using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class RuleSelector<T> : EnumerableNext<Rule<T>>
    {
        public IEnumerable<Rule<T>> Rules { get; private set; }

        public RuleSelector(IEnumerable<Rule<T>> rules)
            : this(rules, rules.Repeat())
        {
        }

        public RuleSelector(IEnumerable<Rule<T>> rules, Func<Rule<T>> next)
            : this(rules, EnumerableExtensions.Repeat(next))
        {
        }

        // ReSharper disable UnusedParameter.Local
        protected RuleSelector(IEnumerable<Rule<T>> rules, bool nextOverridenCtor)
        // ReSharper restore UnusedParameter.Local
            : this(rules, Enumerable.Empty<Rule<T>>())
        {
            Enumerator = EnumerableExtensions.Repeat(Next).GetEnumerator();
        }

        public RuleSelector(IEnumerable<Rule<T>> rules, IEnumerable<Rule<T>> rulesSequence)
            : base(rulesSequence)
        {
            Rules = rules;
        }
    }
}

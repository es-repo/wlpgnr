using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class CircularRuleSelector<T> : RuleSelector<T>
    {
        public CircularRuleSelector(IEnumerable<Rule<T>> rules)
            : base(rules, rules.Repeat())
        {
        }
    }
}

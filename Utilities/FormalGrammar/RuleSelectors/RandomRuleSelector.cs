using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class RandomRuleSelector<T> : RuleSelector<T>
    {
        public RandomRuleSelector(Random random, IEnumerable<Rule<T>> rules)
            : base(rules, () => rules.TakeRandom(random))
        {
        }

        public RandomRuleSelector(Random random, IEnumerable<Rule<T>> rules, IEnumerable<double> probabilities)
            : base(rules, () => rules.TakeRandom(random, probabilities))
        {
        }
    }
}

using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class RandomRuleSelector<T> : RuleSelector<T>
    {
        private readonly Random _random;

        public RandomRuleSelector(Random random, IEnumerable<Rule<T>> rules)
            : base(rules)
        {
            _random = random;
        }

        public override Rule<T> Select()
        {
            return _rules.TakeRandom(_random);
        }
    }
}

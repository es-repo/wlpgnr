using System;
using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class RandomRuleSelector<T> : RuleSelector<T>
    {
        private readonly Random _random;
        private readonly IEnumerable<double> _probabilities;

        public RandomRuleSelector(Random random, IEnumerable<Rule<T>> rules)
            : this(random, rules, null)
        {
        }

        public RandomRuleSelector(Random random, IEnumerable<Rule<T>> rules, IEnumerable<double> probabilities)
            : base(rules)
        {
            _random = random;
            _probabilities = probabilities;
        }

        public override Rule<T> Select()
        {
            return _probabilities == null
                       ? _rules.TakeRandom(_random)
                       : _rules.TakeRandom(_random, _probabilities);
        }
    }
}

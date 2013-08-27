using System.Collections.Generic;
using WallpaperGenerator.Utilities.DataStructures.Collections;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class CircularRuleSelector<T> : RuleSelector<T>
    {
        private readonly CircularEnumeration<Rule<T>> _circularRules;

        public CircularRuleSelector(IEnumerable<Rule<T>> rules)
            : base(rules)
        {
            _circularRules = new CircularEnumeration<Rule<T>>(rules);
        }
        
        public override Rule<T> Next()
        {
            return _circularRules.Next();
        }
    }
}

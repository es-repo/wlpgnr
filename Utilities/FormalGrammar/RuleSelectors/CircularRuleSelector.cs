using System.Collections.Generic;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class CircularRuleSelector<T> : RuleSelector<T>
    {
        private readonly IEnumerator<Rule<T>> _enumerator;
        
        public CircularRuleSelector(IEnumerable<Rule<T>> rules)
            : base(rules)
        {
            _enumerator = rules.Repeat().GetEnumerator(); 
        }
        
        public override Rule<T> Next()
        {
            _enumerator.MoveNext();
            return _enumerator.Current;
        }
    }
}

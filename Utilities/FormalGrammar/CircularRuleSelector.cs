using System.Collections.Generic;
using WallpaperGenerator.Utilities.DataStructures.Collections;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class CircularRuleSelector<T> : RuleSelector<T>
    {
        private readonly Dictionary<Symbol<T>, CircularEnumeration<Rule<T>>> _symbolsAndRules;

        public CircularRuleSelector(Grammar<T> grammar)
            : base(grammar)
        {
            _symbolsAndRules = new Dictionary<Symbol<T>, CircularEnumeration<Rule<T>>>();
        }

        public override Rule<T> SelectRule(Symbol<T> symbol)
        {
            if (!_symbolsAndRules.ContainsKey(symbol))
            {
                IEnumerable<Rule<T>> rules = Grammar.GetRules(symbol);
                _symbolsAndRules.Add(symbol, new CircularEnumeration<Rule<T>>(rules));
            }

            return _symbolsAndRules[symbol].Next;
        }
    }

}

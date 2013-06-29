using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class Grammar<T>
    {
        public IDictionary<string, Symbol<T>> Symbols { get; private set; }

        public Rule<T>[] Rules;

        public Grammar(IEnumerable<Symbol<T>> symbols, Rule<T>[] rules)
        {
            Symbols = new Dictionary<string, Symbol<T>>();
            foreach (Symbol<T> symbol in symbols)
            {
                Symbols.Add(symbol.Name, symbol);
            }

            Rules = rules;
        }

        public IEnumerable<Rule<T>> GetRules(Symbol<T> symbol)
        {
            return Rules.Where(r => r.Left.Name == symbol.Name);
        }
    }
}

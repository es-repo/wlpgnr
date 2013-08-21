using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class Grammar<T>
    {
        public SymbolsSet<T> Symbols { get; private set; }

        public Rule<T>[] Rules;

        public Grammar(SymbolsSet<T> symbols, Rule<T>[] rules)
        {
            Symbols = symbols;
            Rules = rules;
        }

        public IEnumerable<Rule<T>> GetRules(Symbol<T> symbol)
        {
            return Rules.Where(r => r.From.Name == symbol.Name);
        }
    }
}

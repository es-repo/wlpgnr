using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class RuleFactory<T>
    {
        public Dictionary<string, Symbol<T>> Symbols { get; private set; }

        public RuleFactory(IEnumerable<Symbol<T>> symbols)
        {
            Symbols = new Dictionary<string, Symbol<T>>();
            foreach (Symbol<T> symbol in symbols)
            {
                Symbols.Add(symbol.Name, symbol);
            }
        }

        public Rule<T> Create(string left, params string[] right)
        {
            IEnumerable<Symbol<T>> rightSymbols = right.Select(s => Symbols[s]);
            return new Rule<T>(Symbols[left], rightSymbols.ToArray());
        }
    }
}

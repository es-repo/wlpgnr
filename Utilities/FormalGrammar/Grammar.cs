using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Utilities.DataStructures.Collections;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class Grammar<T>
    {
        private readonly KeyedSet<string, Symbol<T>> _symbolsSet; 
        
        public Rule<T>[] Rules;

        public Grammar(Rule<T>[] rules)
        {
            Rules = rules;
            IEnumerable<Symbol<T>> symbols = rules.Select(r => r.From);
            _symbolsSet = new KeyedSet<string, Symbol<T>>(s => s.Name, symbols);
        }

        public IEnumerable<Rule<T>> GetRules(Symbol<T> symbol)
        {
            return GetRules(symbol.Name);
        }

        public IEnumerable<Rule<T>> GetRules(string symbol)
        {
            return Rules.Where(r => r.From.Name == symbol);
        } 

        public IEnumerable<T> GenerateSequence(string startSymbol)
        {
            return GenerateSequence(_symbolsSet[startSymbol]);
        }

        public IEnumerable<T> GenerateSequence(Symbol<T> startSymbol)
        {
            Stack<Symbol<T>> stack = new Stack<Symbol<T>>();
            stack.Push(startSymbol);

            while (stack.Count > 0)
            {
                Symbol<T> currentSymbol = stack.Pop();
                if (currentSymbol.IsTerminal)
                {
                    yield return currentSymbol.Value;
                }
                else
                {
                    Rule<T> rule = GetRules(currentSymbol).First();
                    IEnumerable<Symbol<T>> producedSymbols = rule.Produce();
                    foreach (Symbol<T> symbol in producedSymbols.Reverse())
                    {
                        stack.Push(symbol);
                    }
                }
            }
        }
    }
}

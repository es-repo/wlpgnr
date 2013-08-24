using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class SequenceGenerator<T> : IEnumerable<T>
    {
        public Grammar<T> Grammar { get; private set; }
        public Symbol<T> StartSymbol { get; private set; } 

        public SequenceGenerator(Grammar<T> grammar, Symbol<T> startSymbol)
        {
            Grammar = grammar;
            StartSymbol = startSymbol;
        }

        public SequenceGenerator(Grammar<T> grammar, string startSymbol)
            : this(grammar, grammar.Symbols[startSymbol])
        {
        }

        public IEnumerator<T> GetEnumerator()
        {
            Stack<Symbol<T>> stack = new Stack<Symbol<T>>();
            stack.Push(StartSymbol);

            while (stack.Count > 0)
            {
                Symbol<T> currentSymbol = stack.Pop();
                if (currentSymbol.IsTerminal)
                {
                    yield return currentSymbol.Value;
                }
                else
                {
                    Rule<T> rule = Grammar.GetRules(currentSymbol).First();
                    IEnumerable<Symbol<T>> ruleGeneratedSymbols = rule.Produce();
                    foreach (Symbol<T> symbol in ruleGeneratedSymbols.Reverse())
                    {
                        stack.Push(symbol);
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }    
}

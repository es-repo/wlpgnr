using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Utilities.DataStructures.Collections;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public abstract class RuleSelector<T>
    {
        protected Grammar<T> Grammar { get; private set; }

        protected RuleSelector(Grammar<T> grammar)
        {
            Grammar = grammar;
        }

        public abstract Rule<T> SelectRule(Symbol<T> symbol);
    }

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
    
    public class SequenceGenerator<T>
    {
        private readonly Grammar<T> _grammar;
        private readonly Func<RuleSelector<T>> _createRuleSelector;
        private readonly int _sequenceLengthLimit;

        public SequenceGenerator(Grammar<T> grammar, Func<RuleSelector<T>> createRuleSelector, int sequenceLengthLimit)
        {
            _grammar = grammar;
            _createRuleSelector = createRuleSelector;
            _sequenceLengthLimit = sequenceLengthLimit;
        }
  
        public IEnumerable<T> Generate(string startSymbol)
        {
            return Generate(_grammar.Symbols[startSymbol]);
        }

        public IEnumerable<T> Generate(Symbol<T> startSymbol)
        {
            RuleSelector<T> ruleSelector = _createRuleSelector();

            Stack<Symbol<T>> stack = new Stack<Symbol<T>>();
            stack.Push(startSymbol);

            int i = 0;
            while (stack.Count > 0)
            {
                Symbol<T> currentSymbol = stack.Pop();
                if (currentSymbol.IsTerminal)
                {
                    i++;
                    if (i == _sequenceLengthLimit)
                    {
                        throw new InvalidOperationException("Genereated sequence length has exceeded its limit. There is probably the infinite sequence.");
                    }

                    yield return currentSymbol.Value;
                    
                }
                else
                {
                    Rule<T> rule = ruleSelector.SelectRule(currentSymbol);
                    IEnumerable<Symbol<T>> ruleGeneratedSymbols = rule.Apply();
                    foreach (Symbol<T> symbol in ruleGeneratedSymbols.Reverse())
                    {
                        stack.Push(symbol);
                    }
                }
            } 
        }
    }

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

    public class Rule<T>
    {
        public Symbol<T> Left { get; private set; }

        public Symbol<T>[] Right { get; private set; }

        public Rule(Symbol<T> left, params Symbol<T>[] right)
        {
            Left = left;
            Right = right;
        }

        public IEnumerable<Symbol<T>> Apply()
        {
            return Right;
        }
    }

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

    public class Symbol<T>
    {
        public string Name { get; private set; }

        public T Value { get; private set; }

        public bool IsTerminal { get; private set; }

        public Symbol(string name)
            : this (name, default(T), false)
        {
        }

        public Symbol(string name, T value)
            : this (name, value, true)
        {
        }

        private Symbol(string name, T value, bool isTerminal)
        {
            Name = name;
            Value = value;
            IsTerminal = isTerminal;
        }
    }
}

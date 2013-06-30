﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar
{
    public class SequenceGenerator<T>
    {
        private readonly Grammar<T> _grammar;
        private readonly Func<GrammarRuleSelector<T>> _createRuleSelector;
        private readonly int _sequenceLengthLimit;

        public SequenceGenerator(Grammar<T> grammar, Func<GrammarRuleSelector<T>> createRuleSelector, int sequenceLengthLimit)
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
            GrammarRuleSelector<T> ruleSelector = _createRuleSelector();

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
}
using System;
using System.Linq;
using System.Collections.Generic;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Utilities.DataStructures.Trees.TreeGenerating
{
    public static class TreeGenerator
    {
        public static TreeNode<T> Generate<T>(Grammar<T> grammar, string startSymbol, Func<T, int> getNodeChildrenCount)
        {
            return GenerateInternal(grammar, startSymbol, getNodeChildrenCount);
        }

        public static TreeNode<T> Generate<T>(Grammar<T> grammar, Symbol<T> startSymbol, Func<T, int> getNodeChildrenCount)
        {
            return GenerateInternal(grammar, startSymbol, getNodeChildrenCount);
        }

        private static TreeNode<T> GenerateInternal<T>(Grammar<T> grammar, object startSymbol, Func<T, int> getNodeChildrenCount)
        {
            IEnumerable<TreeGeneratingRuleSelector<T>> treeGeneratingRuleSelectors =
                grammar.Rules.OfType<OrRule<T>>().Select(r => r.RuleSelector).OfType<TreeGeneratingRuleSelector<T>>();

            if (!treeGeneratingRuleSelectors.Any())
                throw new ArgumentException("Grammar should contain at least one rule with TreeGeneratingRuleSelector.", "grammar");

            TreeBuilder<T> treeBuilder = new TreeBuilder<T>();
            foreach (TreeGeneratingRuleSelector<T> s in treeGeneratingRuleSelectors)
            {
                s.TreeBuilder = treeBuilder;
            }
            
            IEnumerable<T> sequence = startSymbol is string 
                ? grammar.GenerateSequence((string)startSymbol) 
                : grammar.GenerateSequence((Symbol<T>)startSymbol);
            return treeBuilder.Append(sequence, getNodeChildrenCount);
        }
    }
}

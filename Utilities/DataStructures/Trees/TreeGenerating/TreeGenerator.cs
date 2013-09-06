using System;
using System.Collections.Generic;
using WallpaperGenerator.Utilities.FormalGrammar;

namespace WallpaperGenerator.Utilities.DataStructures.Trees.TreeGenerating
{
    public static class TreeGenerator
    {
        public static TreeNode<T> Generate<T>(Grammar<T> grammar, string startSymbol, TreeBuilder<T> treeBuilder, Func<T, int> getNodeChildrenCount)
        {
            return GenerateInternal(grammar, startSymbol, treeBuilder, getNodeChildrenCount);
        }

        public static TreeNode<T> Generate<T>(Grammar<T> grammar, Symbol<T> startSymbol, TreeBuilder<T> treeBuilder, Func<T, int> getNodeChildrenCount)
        {
            return GenerateInternal(grammar, startSymbol, treeBuilder, getNodeChildrenCount);
        }

        private static TreeNode<T> GenerateInternal<T>(Grammar<T> grammar, object startSymbol, TreeBuilder<T> treeBuilder, Func<T, int> getNodeChildrenCount)
        {
            IEnumerable<T> sequence = startSymbol is string 
                ? grammar.GenerateSequence((string)startSymbol) 
                : grammar.GenerateSequence((Symbol<T>)startSymbol);
            return treeBuilder.Append(sequence, getNodeChildrenCount);
        }
    }
}

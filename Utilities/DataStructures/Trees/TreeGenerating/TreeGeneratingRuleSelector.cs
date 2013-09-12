using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Utilities.FormalGrammar;

namespace WallpaperGenerator.Utilities.DataStructures.Trees.TreeGenerating
{
    public class TreeGeneratingRuleSelector<T> : RuleSelector<T>
    {
        private readonly Rule<T> _leafProducingRule; 
        private readonly RuleSelector<T> _nonLeafProducingRulesSelector;
        
        public int MinimalTreeDepth { get; private set; }

        public TreeBuilder<T> TreeBuilder { get; set; }

        public TreeGeneratingRuleSelector(int minimalTreeDepth, IEnumerable<Rule<T>> nodeProducingRules,
            Func<IEnumerable<Rule<T>>, RuleSelector<T>> createNonLeafProducingRulesSelector = null)
            : this(minimalTreeDepth, null, nodeProducingRules, createNonLeafProducingRulesSelector)
        {
        }

        public TreeGeneratingRuleSelector(int minimalTreeDepth, TreeBuilder<T> treeBuilder, IEnumerable<Rule<T>> nodeProducingRules,
            Func<IEnumerable<Rule<T>>, RuleSelector<T>> createNonLeafProducingRulesSelector = null)
        {
            if (minimalTreeDepth < 1)
            {
                throw new ArgumentException("Tree depth should be greater then 0", "minimalTreeDepth");
            }

            MinimalTreeDepth = minimalTreeDepth;
            TreeBuilder = treeBuilder; 

            if (createNonLeafProducingRulesSelector == null)
            {
                createNonLeafProducingRulesSelector = rs => new RuleSelector<T>(rs);
            }

            _leafProducingRule = nodeProducingRules.First();
            IEnumerable<Rule<T>> nonLeafProducingRules = nodeProducingRules.Skip(1);
            _nonLeafProducingRulesSelector = createNonLeafProducingRulesSelector(nonLeafProducingRules);
        }

        public override Rule<T> Next()
        {
            if (TreeBuilder == null)
                throw new InvalidOperationException("TreeBuilder is not set.");
            
            if (TreeBuilder.IsTreeReady)
                throw new InvalidOperationException("Tree is already built.");
            
            bool nextIsLeaf = TreeBuilder.NextAppendingNodeInfo.Depth >= MinimalTreeDepth;
            return nextIsLeaf
                ? _leafProducingRule 
                : _nonLeafProducingRulesSelector.Next();
        }
    }
}


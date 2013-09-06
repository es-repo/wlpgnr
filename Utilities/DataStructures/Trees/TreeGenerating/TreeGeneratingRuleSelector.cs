using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;

namespace WallpaperGenerator.Utilities.DataStructures.Trees.TreeGenerating
{
    public class TreeGeneratingRuleSelector<T> : RuleSelector<T>
    {
        private readonly Rule<T> _leafProducingRule; 
        private readonly RuleSelector<T> _nonLeafProducingRulesSelector;
        private readonly TreeBuilder<T> _treeBuilder;
 
        public int MinimalTreeDepth { get; private set; }

        public TreeGeneratingRuleSelector(int minimalTreeDepth, TreeBuilder<T> treeBuilder, IEnumerable<Rule<T>> nodeProducingRules,
            Func<IEnumerable<Rule<T>>, RuleSelector<T>> createNonLeafProducingRulesSelector = null)
            : base(nodeProducingRules)
        {
            if (minimalTreeDepth < 1)
            {
                throw new ArgumentException("Tree depth should be greater then 0", "minimalTreeDepth");
            }

            MinimalTreeDepth = minimalTreeDepth;
            _treeBuilder = treeBuilder; 

            if (createNonLeafProducingRulesSelector == null)
            {
                createNonLeafProducingRulesSelector = rs => new CircularRuleSelector<T>(rs);
            }

            _leafProducingRule = nodeProducingRules.First();
            IEnumerable<Rule<T>> nonLeafProducingRules = nodeProducingRules.Skip(1);
            _nonLeafProducingRulesSelector = createNonLeafProducingRulesSelector(nonLeafProducingRules);
        }

        public override Rule<T> Next()
        {
            if (_treeBuilder.IsTreeReady)
                throw new InvalidOperationException("Tree is already built.");
            
            bool nextIsLeaf = _treeBuilder.NextAppendingNodeInfo.Depth >= MinimalTreeDepth;
            return nextIsLeaf
                ? _leafProducingRule 
                : _nonLeafProducingRulesSelector.Next();
        }
    }
}


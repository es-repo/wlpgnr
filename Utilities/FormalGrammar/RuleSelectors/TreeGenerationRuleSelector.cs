using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Utilities.DataStructures.Trees;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class TreeGenerationRuleSelector<T> : RuleSelector<T>
    {
        private readonly IDictionary<Rule<T>, int> _ruleAndTreeNodeChildrenCountMap;
        private readonly Rule<T> _leafProducingRule; 
        private readonly RuleSelector<T> _nonLeafProducingRulesSelector;
        private TreeBuilder<int> _treeBuilder;
 
        public int TreeDepth { get; private set; }

        public TreeGenerationRuleSelector(int treeDepth, IEnumerable<Rule<T>> nodeProducingRules,
            Func<IEnumerable<Rule<T>>, RuleSelector<T>> createNonLeafProducingRulesSelector = null)
            : base(nodeProducingRules)
        {
            if (treeDepth < 1)
            {
                throw new ArgumentException("Tree depth should be greater then 0", "treeDepth");
            }

            TreeDepth = treeDepth;

            _ruleAndTreeNodeChildrenCountMap = new Dictionary<Rule<T>, int>();
            int a = 0;
            foreach (Rule<T> rule in nodeProducingRules)
            {
                _ruleAndTreeNodeChildrenCountMap.Add(rule, a);
                a++;
            }

            if (createNonLeafProducingRulesSelector == null)
            {
                createNonLeafProducingRulesSelector = rs => new CircularRuleSelector<T>(rs);
            }

            _leafProducingRule = _ruleAndTreeNodeChildrenCountMap.First(e => e.Value == 0).Key; 
            IEnumerable<Rule<T>> nonLeafProducingRules = _ruleAndTreeNodeChildrenCountMap.Where(e => e.Value != 0).Select(e => e.Key);
            _nonLeafProducingRulesSelector = createNonLeafProducingRulesSelector(nonLeafProducingRules);
        }

        public override Rule<T> Next()
        {
            if (_treeBuilder == null || _treeBuilder.IsTreeReady)
            {
                _treeBuilder = new TreeBuilder<int>();
            }
            
            bool nextIsLeaf = _treeBuilder.NextAppendingNodeInfo.Depth == TreeDepth;
            Rule<T> rule = nextIsLeaf
                ? _leafProducingRule 
                : _nonLeafProducingRulesSelector.Next();

            int childNodesCount = _ruleAndTreeNodeChildrenCountMap[rule];
            _treeBuilder.Append(childNodesCount, childNodesCount);
 
            return rule;
        }
    }
}


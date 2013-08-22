using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class TreeGenerationRuleSelector<T> : RuleSelector<T>
    {
        private readonly IDictionary<Rule<T>, int> _rulesAndChildNodesCount;
        private readonly Rule<T> _leafProducingRule; 
        private readonly RuleSelector<T> _nonLeafProducingRulesSelector;

        private readonly Stack<int> _childNodesCountToGenerate;

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

            _rulesAndChildNodesCount = new Dictionary<Rule<T>, int>();
            int a = 0;
            foreach (Rule<T> rule in nodeProducingRules)
            {
                _rulesAndChildNodesCount.Add(rule, a);
                a++;
            }

            if (createNonLeafProducingRulesSelector == null)
            {
                createNonLeafProducingRulesSelector = rs => new CircularRuleSelector<T>(rs);
            }

            _leafProducingRule = _rulesAndChildNodesCount.First(e => e.Value == 0).Key; 
            IEnumerable<Rule<T>> nonLeafProducingRules = _rulesAndChildNodesCount.Where(e => e.Value != 0).Select(e => e.Key);
            _nonLeafProducingRulesSelector = createNonLeafProducingRulesSelector(nonLeafProducingRules);

            _childNodesCountToGenerate = new Stack<int>();
        }

        public override Rule<T> Select()
        {
            if (_childNodesCountToGenerate.Count > 0)
            {
                int count = _childNodesCountToGenerate.Pop();
                if (count > 0)
                {
                    _childNodesCountToGenerate.Push(count - 1);
                }
            }
            
            int currentTreeDepth = _childNodesCountToGenerate.Count;
            bool isLeaf = currentTreeDepth + 1 == TreeDepth;
            if (isLeaf)
            {
                return _leafProducingRule;
            }
            
            Rule<T> rule = _nonLeafProducingRulesSelector.Select();
            int childNodesCount = _rulesAndChildNodesCount[rule];
            _childNodesCountToGenerate.Push(childNodesCount);
            return rule;
        }
    }
}

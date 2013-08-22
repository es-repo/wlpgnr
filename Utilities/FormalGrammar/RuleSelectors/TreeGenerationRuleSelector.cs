using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class TreeGenerationRuleSelector<T> : RuleSelector<T>
    {
        private readonly IDictionary<Rule<T>, int> _rulesAndChildNodesCount;
        private readonly Rule<T> _leafProfudingRule; 
        private readonly RuleSelector<T> _nonLeafProducingRulesSelector;

        private readonly Stack<int> _childNodesCountToGenerate;

        public int TreeDepth { get; private set; }

        public TreeGenerationRuleSelector(int treeDepth, IEnumerable<Rule<T>> nodeProducingRules,
            Func<IEnumerable<Rule<T>>, RuleSelector<T>> createNonLeafProducingRulesSelector = null)
            : base(nodeProducingRules)
        {
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

            _leafProfudingRule = _rulesAndChildNodesCount.First(e => e.Value == 0).Key; 
            IEnumerable<Rule<T>> nonLeafProducingRules = _rulesAndChildNodesCount.Where(e => e.Value != 0).Select(e => e.Key);
            _nonLeafProducingRulesSelector = createNonLeafProducingRulesSelector(nonLeafProducingRules);

            _childNodesCountToGenerate = new Stack<int>();
        }

        public override Rule<T> Select()
        {
            int currentTreeDepth = _childNodesCountToGenerate.Count;
            if (currentTreeDepth + 1 == TreeDepth)
            {
                return _leafProfudingRule;
            }

            Rule<T> rule = currentTreeDepth + 1 == TreeDepth 
                ? _leafProfudingRule
                : _nonLeafProducingRulesSelector.Select();

            if (_childNodesCountToGenerate.Count > 0)
            {
                int count = _childNodesCountToGenerate.Pop();
                if (count > 1)
                {
                    _childNodesCountToGenerate.Push(count - 1);
                }
            }

            int childNodesCount = _rulesAndChildNodesCount[rule];
            _childNodesCountToGenerate.Push(childNodesCount);

            return rule;
        }
    }
}

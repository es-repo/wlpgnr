using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors
{
    public class TreeGenerationRuleSelector<T> : RuleSelector<T>
    {
        private readonly IDictionary<int, Rule<T>> _childNodesCountAndRules;
        private readonly RuleSelector<T> _nonLeafProducingRulesSelector;

        private readonly Stack<int> _childNodesCountToGenerate;

        public int TreeDepth { get; private set; }

        public TreeGenerationRuleSelector(int treeDepth, IEnumerable<Rule<T>> nodeProducingRules,
            Func<IEnumerable<Rule<T>>, RuleSelector<T>> createNonLeafProducingRulesSelector = null)
            : base(nodeProducingRules)
        {
            TreeDepth = treeDepth;

            _childNodesCountAndRules = new Dictionary<int, Rule<T>>();
            int a = 0;
            foreach (Rule<T> rule in nodeProducingRules)
            {
                _childNodesCountAndRules.Add(a, rule);
                a++;
            }

            if (createNonLeafProducingRulesSelector == null)
            {
                createNonLeafProducingRulesSelector = rs => new CircularRuleSelector<T>(rs);
            }

            IEnumerable<Rule<T>> nonLeafProducingRules = _childNodesCountAndRules.Where(e => e.Key != 0).Select(e => e.Value);
            _nonLeafProducingRulesSelector = createNonLeafProducingRulesSelector(nonLeafProducingRules);

            _childNodesCountToGenerate = new Stack<int>();
        }

        public override Rule<T> Select()
        {
            int currentTreeDepth = _childNodesCountToGenerate.Count;
            if (currentTreeDepth + 1 == TreeDepth)
            {
                return _childNodesCountAndRules[0];
            }
            
            throw new NotImplementedException();
        }
    }
}

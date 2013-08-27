using System;
using System.Linq;
using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar.RuleSelectors
{
    [TestFixture]
    public class TreeGenerationRuleSelectorTests
    {        
        [RowTest]
        [Row(0, null, ExpectedException = typeof(ArgumentException))]
        [Row(1, new[] { "Node0" })]
        [Row(2, new[] { "Node1", "Node0" })]
        [Row(3, new[] { "Node1", "Node2", "Node0", "Node0" })]
        [Row(4, new[] { "Node1", "Node2", "Node1", "Node0", "Node2", "Node0", "Node0" })]
        [Row(5, new[] { "Node1", "Node2", "Node1", "Node2", "Node0", "Node0", "Node1" , "Node0" })]
        public void Test(int treeDepth, string[] expectedProducedSymbols)
        {
            SymbolsSet<string> symbols = new SymbolsSet<string>(new[]
            {
                new Symbol<string>("Node0"),
                new Symbol<string>("Node1"),
                new Symbol<string>("Node2"),
                new Symbol<string>("Node")
            });
            
            // Node -> Node0|Node1|Node2
            OrRule<string> nodeProducingRule = new OrRule<string>(symbols["Node"],
                new[] {symbols["Node0"], symbols["Node1"], symbols["Node2"]});

            TreeGenerationRuleSelector<string> ruleSelector = 
                new TreeGenerationRuleSelector<string>(treeDepth, nodeProducingRule.Rules);

            IEnumerable<Rule<string>> rules = ruleSelector.Take(expectedProducedSymbols.Length);

            IEnumerable<Rule<string>> expectedRules = 
                expectedProducedSymbols.Select(s => nodeProducingRule.Rules.First(r => r.Produce().First()  == symbols[s]));

            CollectionAssert.AreEqual(expectedRules.ToArray(), rules.ToArray());
        }
    }
}

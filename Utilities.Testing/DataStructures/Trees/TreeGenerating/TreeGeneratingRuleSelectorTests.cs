using System;
using System.Linq;
using System.Collections.Generic;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Trees;
using WallpaperGenerator.Utilities.DataStructures.Trees.TreeGenerating;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Trees.TreeGenerating
{
    [TestFixture]
    public class TreeGeneratingRuleSelectorTests
    {        
        [RowTest]
        [Row(0, null, null, ExpectedException = typeof(ArgumentException))]
        [Row(1, null, new[] { "Node0" })]
        [Row(2, null, new[] { "Node1", "Node0" })]
        [Row(3, null, new[] { "Node1", "Node2", "Node0", "Node0" })]
        [Row(4, null, new[] { "Node1", "Node2", "Node1", "Node0", "Node2", "Node0", "Node0" })]
        [Row(5, null, new[] { "Node1", "Node2", "Node1", "Node2", "Node0", "Node0", "Node1", "Node2", "Node0", "Node0" })]
        [Row(3, new[] {"Node0"}, new[] { "Node0" })]
        public void Test(int treeDepth, string[] nodeProducingRuleToSymbols, string[] expectedProducedSymbols)
        {
            SymbolsSet<int> s = new SymbolsSet<int>(new []
            {
                new Symbol<int>("Node0"),
                new Symbol<int>("Node1"),
                new Symbol<int>("Node2"),
                new Symbol<int>("Node")
            });
            
            // Node -> Node0|Node1|Node2
            nodeProducingRuleToSymbols = nodeProducingRuleToSymbols ?? new [] {"Node0", "Node1", "Node2"};
            OrRule<int> nodeProducingRule = new OrRule<int>(s["Node"], nodeProducingRuleToSymbols.Select(n => s[n]));

            TreeBuilder<int> treeBuilder = new TreeBuilder<int>();
            TreeGeneratingRuleSelector<int> ruleSelector = new TreeGeneratingRuleSelector<int>(treeDepth, treeBuilder, nodeProducingRule.Rules);

            List<string> producedSymbols = new List<string>();
            while(!treeBuilder.IsTreeReady)
            {
                Rule<int> rule = ruleSelector.Next();
                string sn = rule.Produce().First().Name;
                int v = int.Parse(sn.Replace("Node", "")); 
                treeBuilder.Append(v, v);
                producedSymbols.Add(sn);
            }

            CollectionAssert.AreEqual(expectedProducedSymbols, producedSymbols);
        }
    }
}

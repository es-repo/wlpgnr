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
        [Row(0, null, ExpectedException = typeof(ArgumentException))]
        [Row(1, new[] { "Node0" })]
        [Row(2, new[] { "Node1", "Node0" })]
        [Row(3, new[] { "Node1", "Node2", "Node0", "Node0" })]
        [Row(4, new[] { "Node1", "Node2", "Node1", "Node0", "Node2", "Node0", "Node0" })]
        [Row(5, new[] { "Node1", "Node2", "Node1", "Node2", "Node0", "Node0", "Node1", "Node2", "Node0", "Node0" })]
        public void Test(int treeDepth, string[] expectedProducedSymbols)
        {
            SymbolsSet<int> s = new SymbolsSet<int>(new []
            {
                new Symbol<int>("Node0"),
                new Symbol<int>("Node1"),
                new Symbol<int>("Node2"),
                new Symbol<int>("Node")
            });
            
            // Node -> Node0|Node1|Node2
            OrRule<int> nodeProducingRule = new OrRule<int>(s["Node"],
                new[] {s["Node0"], s["Node1"], s["Node2"]});

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

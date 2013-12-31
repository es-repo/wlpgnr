using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.DataStructures.Trees;
using WallpaperGenerator.Utilities.DataStructures.Trees.TreeGenerating;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Utilities.Testing.DataStructures.Trees.TreeGenerating
{
    [TestFixture]
    public class TreeGeneratorTests
    {
        [RowTest]
        [Row(4, "1 2 1 0 2 0 0")]
        [Row(5, "1 2 1 2 0 0 1 2 0 0")]
        public void TestGenerateTree(int treeDepth, string expectedSequenceString)
        {
            SymbolsSet<string> s = new SymbolsSet<string>(new[]
                {
                    new Symbol<string>("0", "0"),
                    new Symbol<string>("1", "1"),
                    new Symbol<string>("2", "2"),
                    new Symbol<string>("Val0"),
                    new Symbol<string>("Val1"),
                    new Symbol<string>("Val2"),
                    new Symbol<string>("Node0"),
                    new Symbol<string>("Node1"),
                    new Symbol<string>("Node2"),
                    new Symbol<string>("Node")
                });

            Rule<string>[] rules =
            {
                // Val0 -> 0
                new Rule<string>(s["Val0"], new [] { s["0"] }),

                // Val1 -> 1
                new Rule<string>(s["Val1"], new [] { s["1"] }),

                // Val2 -> 2
                new Rule<string>(s["Val2"], new [] { s["2"] }),
                   
                // Node0 -> Val0
                new Rule<string>(s["Node0"], new [] { s["Val0"] }),

                // Node1 -> Val1 Node
                new Rule<string>(s["Node1"], new [] { s["Val1"], s["Node"] }),

                // Node2 -> Val2 Node Node
                new Rule<string>(s["Node2"], new [] { s["Val2"], s["Node"], s["Node"] }),

                // Node -> Node0|Node1|Node2
                new OrRule<string>(s["Node"], rs => new TreeGeneratingRuleSelector<string>(treeDepth, rs),
                    new[] {s["Node0"], s["Node1"], s["Node2"]})
            };

            Grammar<string> grammar = new Grammar<string>(rules);
            TreeNode<string> treeRoot = TreeGenerator.Generate(grammar, "Node", int.Parse);
            IEnumerable<string> traversedTree = Tree<string>.Traverse(treeRoot).Select(ni => ni.Node.Value);
            Assert.AreEqual(expectedSequenceString, string.Join(" ", traversedTree.ToArray()));
        }

        [RowTest]
        [Row(4, "1 2 1 0 1 0")]
        [Row(5, "1 2 1 2 0 0 1 2 0 0")]
        public void TestGenerateTreeWithSeveralTreeGenerationRules(int treeDepth, string expectedSequenceString)
        {
            SymbolsSet<string> s = new SymbolsSet<string>(new[]
                {
                    new Symbol<string>("0", "0"),
                    new Symbol<string>("1", "1"),
                    new Symbol<string>("2", "2"),
                    new Symbol<string>("Val0"),
                    new Symbol<string>("Val1"),
                    new Symbol<string>("Val2"),
                    new Symbol<string>("Node0"),
                    new Symbol<string>("Node1"),
                    new Symbol<string>("Node2"),
                    new Symbol<string>("NodeA"),
                    new Symbol<string>("NodeB")
                });

            Rule<string>[] rules =
            {
                // Val0 -> 0
                new Rule<string>(s["Val0"], new [] { s["0"] }),

                // Val1 -> 1
                new Rule<string>(s["Val1"], new [] { s["1"] }),

                // Val2 -> 2
                new Rule<string>(s["Val2"], new [] { s["2"] }),
                   
                // Node0 -> Val0
                new Rule<string>(s["Node0"], new [] { s["Val0"] }),

                // Node1 -> Val1 Node
                new Rule<string>(s["Node1"], new [] { s["Val1"], s["NodeA"] }),

                // Node2 -> Val2 Node Node
                new Rule<string>(s["Node2"], new [] { s["Val2"], s["NodeB"], s["NodeB"] }),

                // NodeA -> Node0|Node2
                new OrRule<string>(s["NodeA"], rs => new TreeGeneratingRuleSelector<string>(treeDepth, rs),
                    new[] {s["Node0"], s["Node2"]}),

                // NodeB -> Node0|Node1
                new OrRule<string>(s["NodeB"], rs => new TreeGeneratingRuleSelector<string>(treeDepth, rs),
                    new[] {s["Node0"], s["Node1"]})
            };

            Grammar<string> grammar = new Grammar<string>(rules);
            TreeNode<string> treeRoot = TreeGenerator.Generate(grammar, "NodeB", int.Parse);
            IEnumerable<string> traversedTree = Tree<string>.Traverse(treeRoot).Select(ni => ni.Node.Value);
            Assert.AreEqual(expectedSequenceString, string.Join(" ", traversedTree.ToArray()));
        }

        [RowTest]
        [Row(4, "1 2 0 2 0 0")]
        [Row(5, "1 2 0 2 1 2 0 0 1 2 0 0")]
        public void TestGenerateTreeWithSubTreeRules(int treeDepth, string expectedSequenceString)
        {
            SymbolsSet<string> s = new SymbolsSet<string>(new[]
                {
                    new Symbol<string>("0", "0"),
                    new Symbol<string>("1", "1"),
                    new Symbol<string>("2", "2"),
                    new Symbol<string>("Val0"),
                    new Symbol<string>("Val1"),
                    new Symbol<string>("Val2"),
                    new Symbol<string>("SubTree"),
                    new Symbol<string>("Node0"),
                    new Symbol<string>("Node1"),
                    new Symbol<string>("Node2"),
                    new Symbol<string>("NodeA"),
                    new Symbol<string>("NodeB")
                });

            Rule<string>[] rules =
            {
                // Val0 -> 0
                new Rule<string>(s["Val0"], new [] { s["0"] }),

                // Val1 -> 1
                new Rule<string>(s["Val1"], new [] { s["1"] }),

                // Val2 -> 2
                new Rule<string>(s["Val2"], new [] { s["2"] }),
                   
                // Node0 -> Val0
                new Rule<string>(s["Node0"], new [] { s["Val0"] }),

                // Node1 -> Val1 Node
                new Rule<string>(s["Node1"], new [] { s["Val1"], s["Val2"], s["Val0"], s["NodeA"] }),

                // Node2 -> Val2 Node Node
                new Rule<string>(s["Node2"], new [] { s["Val2"], s["NodeB"], s["NodeB"] }),

                // NodeA -> Node0|Node2
                new OrRule<string>(s["NodeA"], rs => new TreeGeneratingRuleSelector<string>(treeDepth, rs),
                    new[] {s["Node0"], s["Node2"]}),

                // NodeB -> Node0|Node1
                new OrRule<string>(s["NodeB"], rs => new TreeGeneratingRuleSelector<string>(treeDepth, rs),
                    new[] {s["Node0"], s["Node1"]})
            };

            Grammar<string> grammar = new Grammar<string>(rules);
            TreeNode<string> treeRoot = TreeGenerator.Generate(grammar, "NodeB", int.Parse);
            IEnumerable<string> traversedTree = Tree<string>.Traverse(treeRoot).Select(ni => ni.Node.Value);
            Assert.AreEqual(expectedSequenceString, string.Join(" ", traversedTree.ToArray()));
        }
    }
}

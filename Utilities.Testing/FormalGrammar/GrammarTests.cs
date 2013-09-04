using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
{
    [TestFixture]
    public class GrammarTests
    {
        [RowTest]
        [Row("A0", "x")]
        [Row("A1", "sin atan sin sum x y")]
        [Row("A2", "sum x y")]
        [Row("Inf", "sum sum sum sum sum sum sum sum sum sum")]
        public void TestGenerateSequence(string startSymbol, string expectedSequence)
        {
            SymbolsSet<string> s = new SymbolsSet<string>(new[]
                {
                    new Symbol<string>("x", "x"),
                    new Symbol<string>("y", "y"),
                    new Symbol<string>("sin", "sin"),
                    new Symbol<string>("atan", "atan"),
                    new Symbol<string>("sum", "sum"),
                    new Symbol<string>("mul", "mul"),
                    new Symbol<string>("A0"),
                    new Symbol<string>("A1"),
                    new Symbol<string>("A2"),
                    new Symbol<string>("Inf")
                });

            Rule<string>[] rules = new []
            {
                // A0 -> x|y
                new OrRule<string>(s["A0"], new [] { s["x"], s["y"] }),
                   
                // A1 -> (sin A1)|(atan A1)|(sin A2)|(atan A2)
                new OrRule<string>(s["A1"], 
                    new [] 
                    { 
                        new Rule<string>(new []{ s["sin"], s["A1"] }),
                        new Rule<string>(new []{ s["atan"], s["A1"] }),
                        new Rule<string>(new []{ s["sin"], s["A2"] }),
                        new Rule<string>(new []{ s["atan"], s["A2"] })
                    }),
                    
                // A2 -> sum A0 A0
                new Rule<string>(s["A2"], 
                    new [] 
                    { 
                        s["sum"], s["A0"], s["A0"] 
                    }),

                // Inf -> sum Inf
                new Rule<string>(s["Inf"], 
                    new [] 
                    { 
                        s["sum"], s["Inf"] 
                    })
            };

            Grammar<string> grammar = new Grammar<string>(rules);
            IEnumerable<string> sequence = grammar.GenerateSequence(startSymbol).Take(10);
            Assert.AreEqual(expectedSequence, string.Join(" ", sequence.ToArray()));
        }

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
            
            Rule<string>[] rules = new[]
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
                new OrRule<string>(s["Node"], rs => new TreeGenerationRuleSelector<string>(treeDepth, rs),
                    new[] {s["Node0"], s["Node1"], s["Node2"]})
            };

            Grammar<string> grammar = new Grammar<string>(rules);
            IEnumerable<string> sequence = grammar.GenerateSequence("Node").AsEnumerable();
            Assert.AreEqual(expectedSequenceString, string.Join(" ", sequence.ToArray()));
        }
    }
}

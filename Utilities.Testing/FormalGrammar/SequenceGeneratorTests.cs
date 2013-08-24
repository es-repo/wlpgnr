using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
{
    [TestFixture]
    public class SequenceGeneratorTests
    {
        [RowTest]
        [Row("A0", "x")]
        [Row("A1", "sin atan sin sum x y")]
        [Row("A2", "sum x y")]
        [Row("Inf", "sum sum sum sum sum sum sum sum sum sum")]
        public void TestGenerate(string startSymbol, string expectedSequence)
        {
            SymbolsSet<string> symbols = new SymbolsSet<string>(new []
            {
                new Symbol<string>("sin", "sin"),
                new Symbol<string>("atan", "atan"),
                new Symbol<string>("sum", "sum"),
                new Symbol<string>("mul", "mul"),
                new Symbol<string>("x", "x"),
                new Symbol<string>("y", "y"),
                new Symbol<string>("3.14", "3.14"),
                new Symbol<string>("A0"),
                new Symbol<string>("A1"),
                new Symbol<string>("A2"),
                new Symbol<string>("Inf")
            });

            Rule<string>[] rules = new []
            {
                // A0 -> x|y
                new OrRule<string>(symbols["A0"], 
                    new [] { symbols["x"], symbols["y"] }),
                   
                // A1 -> (sin A1)|(atan A1)|(sin A2)|(atan A2)
                new OrRule<string>(symbols["A1"], 
                    new [] 
                    { 
                        new Rule<string>(new []{ symbols["sin"], symbols["A1"] }),
                        new Rule<string>(new []{ symbols["atan"], symbols["A1"] }),
                        new Rule<string>(new []{ symbols["sin"], symbols["A2"] }),
                        new Rule<string>(new []{ symbols["atan"], symbols["A2"] })
                    }),
                    
                // A2 -> sum A0 A0
                new Rule<string>(symbols["A2"], 
                    new [] 
                    { 
                        symbols["sum"], symbols["A0"], symbols["A0"] 
                    }),

                // Inf -> sum Inf
                new Rule<string>(symbols["Inf"], 
                    new [] 
                    { 
                        symbols["sum"], symbols["Inf"] 
                    })
            };

            Grammar<string> grammar = new Grammar<string>(symbols, rules);
            SequenceGenerator<string> sequenceGenerator = new SequenceGenerator<string>(grammar, startSymbol);

            IEnumerable<string> sequence = sequenceGenerator.Take(10);
            Assert.AreEqual(expectedSequence, string.Join(" ", sequence.ToArray()));
        }

        [RowTest]
        [Row(4, "1 2 1 0 2 0 0")]
        [Row(5, "1 2 1 2 0 0 1 0")]
        public void TestGenerateTree(int treeDepth, string expectedSequenceString)
        {
            SymbolsSet<string> symbols = new SymbolsSet<string>(new[]
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
                new Rule<string>(symbols["Val0"], new [] { symbols["0"] }),

                // Val1 -> 1
                new Rule<string>(symbols["Val1"], new [] { symbols["1"] }),

                // Val2 -> 2
                new Rule<string>(symbols["Val2"], new [] { symbols["2"] }),
                   
                // Node0 -> Val0
                new Rule<string>(symbols["Node0"], new [] { symbols["Val0"] }),

                // Node1 -> Val1 Node
                new Rule<string>(symbols["Node1"], new [] { symbols["Val1"], symbols["Node"] }),

                // Node2 -> Val2 Node Node
                new Rule<string>(symbols["Node2"], new [] { symbols["Val2"], symbols["Node"], symbols["Node"] }),

                // Node -> Node0|Node1|Node2
                new OrRule<string>(symbols["Node"], rs => new TreeGenerationRuleSelector<string>(treeDepth, rs),
                    new[] {symbols["Node0"], symbols["Node1"], symbols["Node2"]})
            };

            Grammar<string> grammar = new Grammar<string>(symbols, rules);
            SequenceGenerator<string> sequenceGenerator = new SequenceGenerator<string>(grammar, "Node");

            IEnumerable<string> sequence = sequenceGenerator.AsEnumerable();
            Assert.AreEqual(expectedSequenceString, string.Join(" ", sequence.ToArray()));
        }
    }
}

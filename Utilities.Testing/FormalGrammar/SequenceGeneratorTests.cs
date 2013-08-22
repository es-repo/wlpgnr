using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
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
        [Row("Inf", "", ExpectedException = typeof(InvalidOperationException))]
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

            SequenceGenerator<string> sequenceGenerator = new SequenceGenerator<string>(grammar, 100);

            IEnumerable<string> sequence = sequenceGenerator.Generate(startSymbol);
            Assert.AreEqual(expectedSequence, string.Join(" ", sequence.ToArray()));
        }

        [Test]
        public void TestGenerateTree()
        {
            // Val0 -> 0
            // Val1 -> 1
            // Val2 -> 2
            // Node0 -> Val0
            // Node1 -> Val1 Node
            // Node2 -> Val2 Node Node
            // Node -> Node0|Node1|Node2
        }
    }
}

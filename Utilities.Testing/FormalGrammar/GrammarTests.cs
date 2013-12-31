using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
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
        public void TestProduce(string startSymbol, string expectedSequence)
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

            Rule<string>[] rules =
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
            IEnumerable<string> sequence = grammar.Produce(startSymbol).Take(10);
            Assert.AreEqual(expectedSequence, string.Join(" ", sequence.ToArray()));
        }
    }
}

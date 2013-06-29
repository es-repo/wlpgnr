using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;

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
            Symbol<string>[] symbols = new[]
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
                };

            RuleFactory<string> ruleFactory = new RuleFactory<string>(symbols);

            Rule<string>[] rules = new []
                {
                    ruleFactory.Create("A0", "x"),
                    ruleFactory.Create("A0", "y"),
                    ruleFactory.Create("A1", "sin", "A1"),
                    ruleFactory.Create("A1", "atan", "A1"),
                    ruleFactory.Create("A1", "sin", "A2"),
                    ruleFactory.Create("A1", "atan", "A2"),
                    ruleFactory.Create("A2", "sum", "A0", "A0"),
                    ruleFactory.Create("Inf", "sum", "Inf"),
                };

            Grammar<string> grammar = new Grammar<string>(symbols, rules);

            SequenceGenerator<string> sequenceGenerator = new SequenceGenerator<string>(grammar, () => new CircularGrammarRuleSelector<string>(grammar), 100);

            IEnumerable<string> sequence = sequenceGenerator.Generate(startSymbol);
            Assert.AreEqual(expectedSequence, string.Join(" ", sequence.ToArray()));
        }
    }
}

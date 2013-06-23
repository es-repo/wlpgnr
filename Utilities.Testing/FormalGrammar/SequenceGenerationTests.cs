using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
{
    [TestFixture]
    public class SequenceGenerationTests
    {
        [Test]
        public void TestGenerate()
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

            SequenceGenerator<string> sequenceGenerator = new SequenceGenerator<string>(grammar, () => new CircularRuleSelector<string>(grammar), 100);

            Assert.AreEqual("x", string.Join(" ", sequenceGenerator.Generate("A0").ToArray()));
            Assert.AreEqual("x", string.Join(" ", sequenceGenerator.Generate("A0").ToArray()));
            Assert.AreEqual("sin atan sin sum x y", string.Join(" ", sequenceGenerator.Generate("A1").ToArray()));
            Assert.AreEqual("sum x y", string.Join(" ", sequenceGenerator.Generate("A2").ToArray()));

            try
            {
                sequenceGenerator.Generate("Inf").ToArray();
                Assert.Fail(string.Format("{0} exception is expected.", typeof(InvalidOperationException)));
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}

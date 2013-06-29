using System;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar.RuleSelectors
{
    [TestFixture]
    public class RandomRuleSelectorTests
    {
        private readonly Random _random = new Random(7);

        [Test]
        public void TestSelect()
        {
            Rule<string>[] rules = new[]
                {
                    new Rule<string>(new Symbol<string>("A"), new [] { new Symbol<string>("a" ) } ),
                    new Rule<string>(new Symbol<string>("B"), new [] { new Symbol<string>("b" ) } ),
                    new Rule<string>(new Symbol<string>("C"), new [] { new Symbol<string>("c" ) } )
                };

            RandomRuleSelector<string> ruleSelector = new RandomRuleSelector<string>(_random, rules);
            Assert.AreEqual(rules[1], ruleSelector.Select());
            Assert.AreEqual(rules[2], ruleSelector.Select());
            Assert.AreEqual(rules[1], ruleSelector.Select());
            Assert.AreEqual(rules[0], ruleSelector.Select());
        }
    }
}

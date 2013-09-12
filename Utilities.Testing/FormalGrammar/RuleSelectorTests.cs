using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
{
    [TestFixture]
    public class RuleSelectorTests
    {
        [Test]
        public void Test()
        {
            Rule<string>[] rules = new []
                {
                    new Rule<string>(new Symbol<string>("A"), new [] { new Symbol<string>("a" ) } ),
                    new Rule<string>(new Symbol<string>("B"), new [] { new Symbol<string>("b" ) } ),
                    new Rule<string>(new Symbol<string>("C"), new [] { new Symbol<string>("c" ) } )
                };

            RuleSelector<string> ruleSelector = new RuleSelector<string>(rules);
            Assert.AreEqual(rules[0], ruleSelector.Next());
            Assert.AreEqual(rules[1], ruleSelector.Next());
            Assert.AreEqual(rules[2], ruleSelector.Next());
            Assert.AreEqual(rules[0], ruleSelector.Next());
        }
    }
}

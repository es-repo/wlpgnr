using System;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar.RuleSelectors
{
    [TestFixture]
    public class RandomRuleSelectorTests
    {        
        [RowTest]
        [Row(null, new[] { 1, 2, 1, 0 })]
        [Row(new[] { 0.2, 0.7, 0.1 }, new[] { 1, 1, 1, 0, 1, 1, 0, 2, 1, 1, 1, 2, 1, 0, 1, 1 })]
        public void TestSelect(double[] probabilities, int[] expectedIndexes)
        {
            Random random = new Random(7);
            
            Rule<string>[] rules = new[]
                {
                    new Rule<string>(new Symbol<string>("A"), new [] { new Symbol<string>("a" ) } ),
                    new Rule<string>(new Symbol<string>("B"), new [] { new Symbol<string>("b" ) } ),
                    new Rule<string>(new Symbol<string>("C"), new [] { new Symbol<string>("c" ) } )
                };

            RandomRuleSelector<string> ruleSelector = new RandomRuleSelector<string>(random, rules, probabilities);
            for (int i = 0; i < expectedIndexes.Length; i++)
            {
                Rule<string> rule = ruleSelector.Select();
                Assert.AreEqual(rules[expectedIndexes[i]], rule);
            }
        }
    }
}

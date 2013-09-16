using System;
using System.Linq;
using System.Collections.Generic;
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
        //[Row(new[] { 0.2, 0.7, 0.1 }, new[] { 1, 1, 1, 0, 1, 1, 0, 2, 1, 1, 1, 2, 1, 0, 1, 1 })]
        public void Test(double[] probabilities, int[] expectedIndexes)
        {
            Random random = new Random(7);
            
            Rule<string>[] rules = new[]
                {
                    new Rule<string>(new Symbol<string>("A"), new [] { new Symbol<string>("a" ) } ),
                    new Rule<string>(new Symbol<string>("B"), new [] { new Symbol<string>("b" ) } ),
                    new Rule<string>(new Symbol<string>("C"), new [] { new Symbol<string>("c" ) } )
                };

            RandomRuleSelector<string> ruleSelector = probabilities == null
                ? new RandomRuleSelector<string>(random, rules)
                : new RandomRuleSelector<string>(random, rules, probabilities);
            IEnumerable<Rule<string>> selectedRules = ruleSelector.Take(expectedIndexes.Length);
            IEnumerable<Rule<string>> expectedRules = expectedIndexes.Select(i => rules[i]);
            CollectionAssert.AreEqual(expectedRules.ToArray(), selectedRules.ToArray());
        }
    }
}

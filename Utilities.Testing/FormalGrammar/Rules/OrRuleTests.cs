using NUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.Rules;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar.Rules
{
    [TestFixture]
    public class OrRuleTests
    {
        [Test]
        public void TestApply()
        {
            Symbol<string> a = new Symbol<string>("a", "a");
            Symbol<string> b = new Symbol<string>("b", "b");
            Symbol<string> c = new Symbol<string>("c", "c");
            Symbol<string> d = new Symbol<string>("d", "d");

            RuleAssert.AssertGeneratedSequences(
                
                // R -> (ab)|(cd)
                new OrRule<string>(rules => new RuleSelector<string>(rules),
                    new Rule<string>(new[] { a, b }),
                    new Rule<string>(new[] { c, d })),

                new[]
                {
                    new[] {a, b},
                    new[] {c, d}
                });

            RuleAssert.AssertGeneratedSequences(

                // R -> a|b|c|d
                new OrRule<string>(rules => new RuleSelector<string>(rules), new[] { a, b, c, d }),

                new[]
                {
                    new[] {a}, 
                    new[] {b}, 
                    new[] {c}, 
                    new[] {d}
                });
        }
    }
}

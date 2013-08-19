using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
{
    [TestFixture]
    public class AndRuleTests
    {
        [Test]
        public void TestApply()
        {
            Symbol<string> a = new Symbol<string>("a", "a");
            Symbol<string> b = new Symbol<string>("b", "b");
            Symbol<string> c = new Symbol<string>("c", "c");
            Symbol<string> d = new Symbol<string>("d", "d");

            RuleAssert.AssertGeneratedSequences(

                // R -> (a|b)(c|d)
                new AndRule<string>(
                    new OrRule<string>(
                        new Rule<string>(new[] {a}),
                        new Rule<string>(new[] {b})),
                    new OrRule<string>(
                        new Rule<string>(new[] {c}),
                        new Rule<string>(new[] {d}))),

                new[]
                {
                    new[] {a, c},
                    new[] {b, d}
                });
        }
    }
}

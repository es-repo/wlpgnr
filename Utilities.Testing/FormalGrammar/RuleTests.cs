using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
{
    [TestFixture]
    public class RuleTests
    {
        [Test]
        public void TestApply()
        {
            Rule<string> rule = new Rule<string>(new Symbol<string>("A"),
                new[] { new Symbol<string>("a", "a"), new Symbol<string>("b", "b"), new Symbol<string>("c", "c") });

            IEnumerable<Symbol<string>> to = rule.Apply();
            string[] sequence = to.Select(s => s.Value).ToArray();
            string[] expectedSequence = new [] { "a", "b", "c"};
            CollectionAssert.AreEqual(expectedSequence, sequence);
        }

        [Test]
        public void TestApplyWithSpecificFunc()
        {
            Rule<int> rule = new Rule<int>(new Symbol<int>("A"), 
                () => Enumerable.Repeat(0, 5).Select((e, i) => (i * 2)).Select(v => new Symbol<int>("a", v)));

            IEnumerable<Symbol<int>> to = rule.Apply();
            int[] sequence = to.Select(s => s.Value).ToArray();
            int[] expectedSequence = new [] {0, 2, 4, 6, 8};
            CollectionAssert.AreEqual(expectedSequence, sequence);
        }

        [Test]
        public void TestCompositeRules()
        {            
            Symbol<string> a = new Symbol<string>("a", "a");
            Symbol<string> b = new Symbol<string>("b", "b");
            Symbol<string> c = new Symbol<string>("c", "c");
            Symbol<string> d = new Symbol<string>("d", "d");
            
            RuleAssert.AssertGeneratedSequences(

                // R -> (aa|bb)(cc|dd)
                Rule<string>.And(
                    Rule<string>.Or(
                        new Rule<string>(new[] { a, a }),
                        new Rule<string>(new[] { b, b })),
                    Rule<string>.Or(
                        new Rule<string>(new[] { c, c }),
                        new Rule<string>(new[] { d, d }))),

                    new[]
                    {
                        new[] {a, a, c, c},
                        new[] {b, b, d, d}
                    });
        }
    }
}

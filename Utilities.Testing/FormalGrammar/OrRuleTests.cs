using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
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
            
            // R -> (ab)|(cd)
            OrRule<string> rule = new OrRule<string>(rules => new CircularRuleSelector<string>(rules),
                    new Rule<string>( new[] { a, b }),
                    new Rule<string>( new[] { c, d }));

            Symbol<string>[][] expectedTos = new []
                {
                    new[] {a, b},
                    new[] {c, d}
                };

            foreach (Symbol<string>[] expectedTo in expectedTos)
            {
                IEnumerable<Symbol<string>> to = rule.Apply();
                CollectionAssert.AreEqual(expectedTo, to.ToArray());
            }
        }
    }
}

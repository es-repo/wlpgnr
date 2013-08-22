using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar
{
    public static class RuleAssert
    {
        public static void AssertGeneratedSequences<T>(Rule<T> rule, IEnumerable<Symbol<T>[]> expectedSequences)
        {
            foreach (Symbol<T>[] expectedSequence in expectedSequences)
            {
                IEnumerable<Symbol<T>> sequence = rule.Produce();
                CollectionAssert.AreEqual(expectedSequence, sequence.ToArray());
            }
        }
    }
}

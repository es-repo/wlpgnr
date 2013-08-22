using System;
using MbUnit.Framework;
using WallpaperGenerator.Utilities.FormalGrammar;
using WallpaperGenerator.Utilities.FormalGrammar.RuleSelectors;

namespace WallpaperGenerator.Utilities.Testing.FormalGrammar.RuleSelectors
{
    [TestFixture]
    public class TreeGenerationRuleSelectorTests
    {        
        [RowTest]
        [Row(null, new[] { 1, 2, 1, 0 })]
        [Row(new[] { 0.2, 0.7, 0.1 }, new[] { 1, 1, 1, 0, 1, 1, 0, 2, 1, 1, 1, 2, 1, 0, 1, 1 })]
        public void TestSelect(double[] probabilities, int[] expectedIndexes)
        {
            // Node -> Node0|Node2
            // Node0 -> 0
            // Node2 -> 2
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [RowTest]
        [Row(new int[]{}, new int[]{})]
        [Row(new [] {1, 2, 3, 4}, new [] {3, 5, 8, 12 })]
        public void TestSelectWithFolding(int[] source, int[] expectedSequence)
        {
            IEnumerable<int> foldedSequence = source.SelectWithFolding((p, c) => p + c, 2);
            CollectionAssert.AreEqual(expectedSequence, foldedSequence.ToArray());
        }

        [RowTest]
        [Row(new[] { 1, 2, 3 }, new[]{0.1, 0.3, 0.6}, new [] {2, 2, 2, 3, 3, 3, 2, 3, 3, 2, 3, 2, 1, 1, 3})]
        [Row(new[] { 1, 2, 3 }, new[] { 0.1, 0.3, 0.7 }, new []{1}, ExpectedException=typeof(ArgumentException))]
        [Row(new[] { 1, 2, 3 }, new[] { 0.1, 0.3, 0.3, 0.3}, new []{1}, ExpectedException = typeof(ArgumentException))]
        public void TestTakeRandomWithProbabilites(int[] source, double[] probabilities, int[] expectedSequence)
        {
            Random random = new Random(5);
            IEnumerable<int> sequence = EnumerableExtensions.Repeat(i => source.TakeRandom(random, probabilities), expectedSequence.Length);
            CollectionAssert.AreEqual(expectedSequence, sequence.ToArray()); 
        }
    }
}

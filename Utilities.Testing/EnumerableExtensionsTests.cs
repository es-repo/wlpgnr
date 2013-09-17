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
        [Row(new [] {1, 2, 3}, new [] {1, 2, 3, 1, 2, 3, 1})]
        [Row(new [] { 1 }, new[] { 1, 1, 1 })]
        [Row(new int [] {}, new [] { 0 }, ExpectedException = typeof(ArgumentException))]
        public void TestRepeatSequence(int[] sequence, int[] expectedSequence)
        {
            IEnumerable<int> loopedSequence = sequence.Repeat().Take(expectedSequence.Length);
            CollectionAssert.AreEqual(expectedSequence, loopedSequence.ToArray());
        }

        [Test]
        public void TestRepeat()
        {
            IEnumerable<int> sequence = EnumerableExtensions.Repeat(i => i, 3); 
            int[] expectedSequence = new []{0, 1, 2};
            CollectionAssert.AreEqual(expectedSequence, sequence.ToArray());
        }

        [RowTest]
        [Row(new int[]{}, new int[]{})]
        [Row(new [] {1, 2, 3, 4}, new [] {3, 5, 8, 12 })]
        public void TestSelectWithFolding(int[] source, int[] expectedSequence)
        {
            IEnumerable<int> foldedSequence = source.SelectWithFolding((p, c) => p + c, 2);
            CollectionAssert.AreEqual(expectedSequence, foldedSequence.ToArray());
        }

        [RowTest]
        [Row(new[] { 1, 2, 3 }, new[] {0.1, 0.3, 0.6}, new[] {0.0, 0.1, 0.39, 0.59, 0.99, 0.07}, new [] {1, 2, 2, 3, 3, 1})]
        [Row(new[] { -1, 0, 1, 2, 3, 4, 5, 6, 7 }, new[] { 0.0, 0.0, 0.3, 0, 0, 0.1, 0.1, 0, 0.5 }, new[] { 0.0, 0.3, 0.0, 0.31, 0.29, 0.4, 0.5, 0.51, 0.7 }, 
            new[] { 1, 4, 1, 4, 1, 5, 7, 7, 7 })]
        [Row(new[] { 1, 2, 3, 4 }, new[] { 1.0, 1.0, 1.0, 1.0 }, new[]{ 0.24, 0.49, 0.74, 0.99 }, new []{1, 2, 3, 4})]
        [Row(new[] { 1, 2, 3 }, new[] { 0.1, 0.3, 0.3, 0.3}, new[]{ 0.0 }, new []{0}, ExpectedException = typeof(ArgumentException))]
        public void TestTakeRandomWithProbabilites(int[] source, double[] probabilities, double[] randomSequence, int[] expectedSequence)
        {
            Random random = RandomMock.Setup(randomSequence);
            IEnumerable<int> sequence = EnumerableExtensions.Repeat(i => source.TakeRandom(random, probabilities), expectedSequence.Length).ToArray();
            CollectionAssert.AreEqual(expectedSequence, sequence.ToArray()); 
        }

    }
}

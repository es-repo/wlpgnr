using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [TestCase(new [] {1, 2, 3}, new [] {1, 2, 3, 1, 2, 3, 1})]
        [TestCase(new [] { 1 }, new[] { 1, 1, 1 })]
        public void TestRepeatSequence(int[] sequence, int[] expectedSequence)
        {
            int[] loopedSequence = sequence.Repeat().Take(expectedSequence.Length).ToArray();
            CollectionAssert.AreEqual(expectedSequence, loopedSequence);
        }

        [Test]
        public void TestRepeat()
        {
            IEnumerable<int> sequence = EnumerableExtensions.Repeat(i => i, 3); 
            int[] expectedSequence = {0, 1, 2};
            CollectionAssert.AreEqual(expectedSequence, sequence);
        }

        [TestCase(new int[]{}, new int[]{})]
        [TestCase(new [] {1, 2, 3, 4}, new [] {3, 5, 8, 12 })]
        public void TestSelectWithFolding(int[] source, int[] expectedSequence)
        {
            IEnumerable<int> foldedSequence = source.SelectWithFolding((p, c) => p + c, 2);
            CollectionAssert.AreEqual(expectedSequence, foldedSequence);
        }

        [TestCase(new[] { 1, 2, 3 }, new[] {0.1, 0.3, 0.6}, new[] {0.0, 0.1, 0.39, 0.59, 0.99, 0.07}, new [] {1, 2, 2, 3, 3, 1})]
        [TestCase(new[] { -1, 0, 1, 2, 3, 4, 5, 6, 7 }, new[] { 0.0, 0.0, 0.3, 0, 0, 0.1, 0.1, 0, 0.5 }, new[] { 0.0, 0.3, 0.0, 0.31, 0.29, 0.4, 0.5, 0.51, 0.7 }, 
            new[] { 1, 4, 1, 4, 1, 5, 7, 7, 7 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1.0, 1.0, 1.0, 1.0 }, new[]{ 0.24, 0.49, 0.74, 0.99 }, new []{1, 2, 3, 4})]
        public void TestTakeRandomWithProbabilites(int[] source, double[] probabilities, double[] randomSequence, int[] expectedSequence)
        {
            Random random = RandomMock.Setup(randomSequence);
            IEnumerable<int> sequence = EnumerableExtensions.Repeat(i => source.TakeRandom(random, probabilities), expectedSequence.Length).ToArray();
            CollectionAssert.AreEqual(expectedSequence, sequence); 
        }

        [Test]
        public void TestTakeRandomWithProbabilitesFailed()
        {
            Assert.Throws<ArgumentException>(() => new[] { 1 }.TakeRandom(new Random(), new[] { 0.2, 0.4 }));
        }
    }
}

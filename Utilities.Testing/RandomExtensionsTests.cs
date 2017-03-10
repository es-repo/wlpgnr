using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class RandomExtensionsTests
    {
        [TestCase(0.9)]
        [TestCase(0.1)]
        [TestCase(1)]
        [TestCase(0)]
        public void TestGetRandomBetweenTwo(double expectedSecondProbability)
        {
            Random random = new Random();
            const int first = 1;
            const int second = 2;
            const int count = 100;
            List<int> seq = new List<int>();
            for (int i = 0; i < count; i++)
            {
                seq.Add(random.GetRandomBetweenTwo(first, second, expectedSecondProbability));
            }

            double secondProbability = (double)seq.Count(a => a == second) / count;
            bool eqaulWithInaccuracy = !expectedSecondProbability.Equals(0) && !expectedSecondProbability.Equals(1);
            if (eqaulWithInaccuracy)
            {
                const double inaccuracy = 0.1;
                Assert.That(secondProbability, 
                    Is.InRange(expectedSecondProbability - inaccuracy, expectedSecondProbability + inaccuracy));
            }
            else
            {
                Assert.AreEqual(expectedSecondProbability, secondProbability);
            }
        }

        [TestCase(0.3, 0.3)]
        [TestCase(0.5, 0.5)]
        [TestCase(1, 0)]
        [TestCase(0, 1)]
        [TestCase(0, 0)]
        public void TestGetRandomBetweenThree(double expectedSecondProbability, double expectedThirdProbability)
        {
            Random random = new Random();
            const int first = 1;
            const int second = 2;
            const int third = 3;
            const int count = 100;
            List<int> seq = new List<int>();
            for (int i = 0; i < count; i++)
            {
                seq.Add(random.GetRandomBetweenThree(first, second, third, expectedSecondProbability, expectedThirdProbability));
            }

            double secondProbability = (double)seq.Count(a => a == second) / count;
            double thirdProbability = (double)seq.Count(a => a == third) / count;
            bool eqaulWithInaccuracy = !expectedSecondProbability.Equals(0) && !expectedSecondProbability.Equals(1);
            if (eqaulWithInaccuracy)
            {
                const double inaccuracy = 0.15;
                Assert.That(secondProbability,
                    Is.InRange(expectedSecondProbability - inaccuracy, expectedSecondProbability + inaccuracy));
            }
            else
            {
                Assert.AreEqual(expectedSecondProbability, secondProbability);
                Assert.AreEqual(expectedThirdProbability, thirdProbability);
            }
        }

        [TestCase(new int[] {}, 0, new int[] {})]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, 0, new int[] {})]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, 1, new [] { 5 })]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, 3, new[] { 5, 1, 4 })]
        [TestCase(new[] { 1, 2, 3, 4, 5 }, 5, new[] { 5, 1, 4, 3, 2 })]
        public void TestTakeDistinctRandom(int[] source, int count, int[] expected)
        {
            Random random = RandomMock.Setup(new[] {0.9, 0.1, 0.8});
            int[] result = random.TakeDistinct(source, count).ToArray();
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void TestTakeDistinctRandomFailed()
        {
            Assert.Throws<ArgumentException>(() => new Random().TakeDistinct(new[] { 1 }, 2).ToArray());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class RandomExtensionsTests
    {
        [RowTest]
        [Row(0.9)]
        [Row(0.1)]
        [Row(1)]
        [Row(0)]
        public void TestGetRandomBetweenTwo(double expectedSecondProbability)
        {
            Random random = new Random();
            const int first = 2;
            const int second = 3;
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
                Assert.Between(secondProbability, expectedSecondProbability - inaccuracy, expectedSecondProbability + inaccuracy);
            }
            else
            {
                Assert.AreEqual(expectedSecondProbability, secondProbability);
            }
        }
    }
}

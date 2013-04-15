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
                Assert.Between(secondProbability, expectedSecondProbability - inaccuracy, expectedSecondProbability + inaccuracy);
            }
            else
            {
                Assert.AreEqual(expectedSecondProbability, secondProbability);
            }
        }

        [RowTest]
        [Row(0.3, 0.3)]
        [Row(0.5, 0.5)]
        [Row(1, 0)]
        [Row(0, 1)]
        [Row(0, 0)]
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
                const double inaccuracy = 0.1;
                Assert.Between(secondProbability, expectedSecondProbability - inaccuracy, expectedSecondProbability + inaccuracy);
                Assert.Between(thirdProbability, expectedThirdProbability - inaccuracy, expectedThirdProbability + inaccuracy);
            }
            else
            {
                Assert.AreEqual(expectedSecondProbability, secondProbability);
                Assert.AreEqual(expectedThirdProbability, thirdProbability);
            }
        }
    }
}

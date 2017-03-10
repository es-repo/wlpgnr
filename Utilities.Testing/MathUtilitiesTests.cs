using NUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class MathUtilitiesTests
    {
        [TestCase(new float[] { }, 0, 0, 0)]
        [TestCase(new float[] { }, 2, 0, 0)]
        [TestCase(new float[] { 2 }, 0, 2, 4)]
        [TestCase(new float[] { 2 }, 2, 2, 4)]
        [TestCase(new float[] { 1, 2 }, 2, 3, 5)]
        [TestCase(new float[] { 1, 2, 3 }, 2, 6, 14)]
        [TestCase(new float[] { 1, 2, 3, 4 }, 2, 10, 30)]
        [TestCase(new float[] { 1, 2, 3, 4 }, 3, 10, 30)]
        public void TestSum(float[] values, int threadsCount, double expectedSum, double expectedSumOfSquares)
        {
            float sum = MathUtilities.Sum(values, null, threadsCount);
            Assert.AreEqual(expectedSum, sum);

            float sumOfSquares = MathUtilities.Sum(values, v => v * v, threadsCount);
            Assert.AreEqual(expectedSumOfSquares, sumOfSquares);
        }

        [TestCase(new float[] { 1 }, 1)]
        [TestCase(new float[] { 1, 2, 3, 4, 5, 6 }, 3.5)]
        [TestCase(new float[] { -1, 0, 1 }, 0)]
        [TestCase(new float[] { 1, 2, 4, 5, 6 }, 3.6)]
        public void TestMathExpectation(float[] values, double expectedMathExpectation)
        {
            double mathExpectation = MathUtilities.MathExpectation(values, 1);
            Assert.AreEqual(expectedMathExpectation, mathExpectation);
        }

        [Test]
        [TestCase(new float[] { }, double.NaN)]
        [TestCase(new float[] { 1 }, 0)]
        [TestCase(new float[] { 1, 2, 3, 4, 5, 6 }, 2.92)]
        [TestCase(new float[] { -1, 0, 1 }, 0.67)]
        [TestCase(new float[] { 1, 2, 4, 5, 6 }, 3.44)]
        public void TestVariance(float[] values, double expectedVariance)
        {
            double variance = MathUtilities.Variance(values, 1);
            Assert.AreEqual(expectedVariance, variance, 0.01);
        }

        [TestCase(new float[] { }, double.NaN)]
        [TestCase(new float[] { 1 }, 0)]
        [TestCase(new float[] { 1, 2, 3, 4, 5, 6 }, 1.71)]
        [TestCase(new float[] { -1, 0, 1 }, 0.82)]
        [TestCase(new float[] { 1, 2, 4, 5, 6 }, 1.85)]
        public void TestStandardDeviation(float[] values, double expectedStandardDeviation)
        {
            double standardDeviation = MathUtilities.StandardDeviation(values, 1);
            Assert.AreEqual(expectedStandardDeviation, standardDeviation, 0.01);
        }

        [TestCase(0, 0, 1, 0, 1, 0)]
        [TestCase(0, 0, 1, 1, 2, 1)]
        [TestCase(-10, -11, -10, 0, 1, 1)]
        [TestCase(3, 0, 10, 0, 100, 30)]
        [TestCase(-24, -30, -10, 400, 500, 430)]
        public void TestMap(double value, double rangeStart, double rangeEnd, double mappedRangeStart, double mappedRangeEnd, double expectedMappedValue)
        {
            double range = rangeEnd - rangeStart;
            double mappedRange = mappedRangeEnd - mappedRangeStart;
            double scale = mappedRange/range; 
            double mappedValue = MathUtilities.Map(value, rangeStart, rangeEnd, range,
                mappedRangeStart, mappedRangeEnd, mappedRange, scale);
            Assert.AreEqual(expectedMappedValue, mappedValue);
        }
    }
}

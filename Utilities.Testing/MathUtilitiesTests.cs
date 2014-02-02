using MbUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class MathUtilitiesTests
    {
        [Test]
        [Row(new float[] { }, 0, 0, 0)]
        [Row(new float[] { }, 2, 0, 0)]
        [Row(new float[] { 2 }, 0, 2, 4)]
        [Row(new float[] { 2 }, 2, 2, 4)]
        [Row(new float[] { 1, 2 }, 2, 3, 5)]
        [Row(new float[] { 1, 2, 3 }, 2, 6, 14)]
        [Row(new float[] { 1, 2, 3, 4 }, 2, 10, 30)]
        [Row(new float[] { 1, 2, 3, 4 }, 3, 10, 30)]
        public void TestSum(float[] values, int threadsCount, double expectedSum, double expectedSumOfSquares)
        {
            float sum = MathUtilities.Sum(values, null, threadsCount);
            Assert.AreEqual(expectedSum, sum);

            float sumOfSquares = MathUtilities.Sum(values, v => v * v, threadsCount);
            Assert.AreEqual(expectedSumOfSquares, sumOfSquares);
        }

        [Test]
        [Row(new float[] { 1 }, 1)]
        [Row(new float[] { 1, 2, 3, 4, 5, 6 }, 3.5)]
        [Row(new float[] { -1, 0, 1 }, 0)]
        [Row(new float[] { 1, 2, 4, 5, 6 }, 3.6)]
        public void TestMathExpectation(float[] values, double expectedMathExpectation)
        {
            double mathExpectation = MathUtilities.MathExpectation(values, 1);
            Assert.AreEqual(expectedMathExpectation, mathExpectation);
        }

        [Test]
        [Row(new float[] { }, double.NaN)]
        [Row(new float[] { 1 }, 0)]
        [Row(new float[] { 1, 2, 3, 4, 5, 6 }, 3.5)]
        [Row(new float[] { -1, 0, 1 }, 1)]
        [Row(new float[] { 1, 2, 4, 5, 6 }, 4.3)]
        public void TestVariance(float[] values, double expectedVariance)
        {
            double variance = MathUtilities.Variance(values, 1);
            Assert.AreApproximatelyEqual(expectedVariance, variance, 0.00000001);
        }

        [Test]
        [Row(new float[] { }, double.NaN)]
        [Row(new float[] { 1 }, 0)]
        [Row(new float[] { 1, 2, 3, 4, 5, 6 }, 1.870828693386971)]
        [Row(new float[] { -1, 0, 1 }, 1)]
        [Row(new float[] { 1, 2, 4, 5, 6 }, 2.073644135332772)]
        public void TestStandardDeviation(float[] values, double expectedStandardDeviation)
        {
            double standardDeviation = MathUtilities.StandardDeviation(values, 1);
            Assert.AreApproximatelyEqual(expectedStandardDeviation, standardDeviation, 0.00000001);
        }

        [Test]
        [Row(0, 0, 1, 0, 1, 0)]
        [Row(0, 0, 1, 1, 2, 1)]
        [Row(-10, -11, -10, 0, 1, 1)]
        [Row(3, 0, 10, 0, 100, 30)]
        [Row(-24, -30, -10, 400, 500, 430)]
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

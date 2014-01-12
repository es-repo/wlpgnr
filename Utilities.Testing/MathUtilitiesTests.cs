using MbUnit.Framework;

namespace WallpaperGenerator.Utilities.Testing
{
    [TestFixture]
    public class MathUtilitiesTests
    {
        [Test]
        [Row(new double[] { 1 }, 1)]
        [Row(new double[] { 1, 2, 3, 4, 5, 6 }, 3.5)]
        [Row(new double[] { -1, 0, 1 }, 0)]
        [Row(new double[] { 1, 2, 4, 5, 6 }, 3.6)]
        public void TestMathExpectation(double[] values, double expectedMathExpectation)
        {
            double mathExpectation = MathUtilities.MathExpectation(values);
            Assert.AreEqual(expectedMathExpectation, mathExpectation);
        }

        [Test]
        [Row(new double[] { }, double.NaN)]
        [Row(new double[] { 1 }, 0)]
        [Row(new double[] { 1, 2, 3, 4, 5, 6 }, 3.5)]
        [Row(new double[] { -1, 0, 1 }, 1)]
        [Row(new double[] { 1, 2, 4, 5, 6 }, 4.3)]
        public void TestVariance(double[] values, double expectedVariance)
        {
            double variance = MathUtilities.Variance(values);
            Assert.AreApproximatelyEqual(expectedVariance, variance, 0.00000001);
        }

        [Test]
        [Row(new double[] { }, double.NaN)]
        [Row(new double[] { 1 }, 0)]
        [Row(new double[] { 1, 2, 3, 4, 5, 6 }, 1.870828693386971)]
        [Row(new double[] { -1, 0, 1 }, 1)]
        [Row(new double[] { 1, 2, 4, 5, 6 }, 2.073644135332772)]
        public void TestStandardDeviation(double[] values, double expectedStandardDeviation)
        {
            double standardDeviation = MathUtilities.StandardDeviation(values);
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

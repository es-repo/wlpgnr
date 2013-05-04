using MbUnit.Framework;  

namespace WallpaperGenerator.FormulaRendering.Testing
{
    [TestFixture]
    public class MathUtilitiesTests
    {
        [RowTest]
        [Row(new double[] { 1 }, 1)]
        [Row(new double[] { 1, 2, 3, 4, 5, 6 }, 3.5)]
        [Row(new double[] { -1, 0, 1 }, 0)]
        [Row(new double[] { 1, 2, 4, 5, 6 }, 3.6)]
        [Row(new[] { double.MaxValue, double.MaxValue }, double.MaxValue)]
        [Row(new[] { double.MaxValue, 0 }, double.MaxValue / 2)]
        public void TestMathExpectation(double[] values, double expectedMathExpectation)
        {
            double mathExpectation = MathUtilities.MathExpectation(values);
            Assert.AreEqual(expectedMathExpectation, mathExpectation);
        }

        [RowTest]
        [Row(new double[] { }, double.NaN)]
        [Row(new double[] { 1 }, 0)]
        [Row(new double[] { 1, 2, 3, 4, 5, 6 }, 3.5)]
        [Row(new double[] { -1, 0, 1 }, 1)]
        [Row(new double[] { 1, 2, 4, 5, 6 }, 4.3)]
        public void TestVariance(double[] values, double expectedVariance)
        {
            double variance = MathUtilities.Variance(values);
            Assert.AreEqual(expectedVariance, variance);
        }

        [RowTest]
        [Row(new double[] { }, double.NaN)]
        [Row(new double[] { 1 }, 0)]
        [Row(new double[] { 1, 2, 3, 4, 5, 6 }, 1.870828693386971)]
        [Row(new double[] { -1, 0, 1 }, 1)]
        [Row(new double[] { 1, 2, 4, 5, 6 }, 2.073644135332772)]
        public void TestStandardDeviation(double[] values, double expectedStandardDeviation)
        {
            double standardDeviation = MathUtilities.StandardDeviation(values);
            Assert.AreEqual(expectedStandardDeviation, standardDeviation);
        }

        [RowTest]
        [Row(new double[] { }, double.NaN)]
        [Row(new double[] { 1 }, 0)]
        [Row(new double[] { 1, 2, 3, 4, 5, 6 }, 1.870828693386971 * 3)]
        [Row(new double[] { -1, 0, 1 }, 3)]
        public void TestThreeSigmas(double[] values, double expectedThreeSigmas)
        {
            double threeSigmas = MathUtilities.ThreeSigmas(values);
            Assert.AreEqual(expectedThreeSigmas, threeSigmas);
        }

        [RowTest]
        [Row(0, 0, 1, 0, 1, 0)]
        [Row(0, 0, 1, 1, 2, 1)]
        [Row(-10, -11, -10, 0, 1, 1)]
        [Row(3, 0, 10, 0, 100, 30)]
        [Row(double.NaN, 0, 10, 0, 100, 50)]
        [Row(-24, -30, -10, 400, 500, 430)]
        public void TestMap(double value, double rangeStart, double rangeEnd, double mappedRangeStart, double mappedRangeEnd, double expectedMappedValue)
        {
            double mappedValue = MathUtilities.Map(value, rangeStart, rangeEnd, mappedRangeStart, mappedRangeEnd);
            Assert.AreEqual(expectedMappedValue, mappedValue);
        }
    }
}

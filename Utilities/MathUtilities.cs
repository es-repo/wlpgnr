using System;
using System.Linq;

namespace WallpaperGenerator.Utilities
{
    public static class MathUtilities
    {
        public static double MathExpectation(double[] values)
        {
            double sum = values.Sum();
            return sum / values.Length;
        }

        public static double Variance(double[] values)
        {
            if (values.Length == 1)
                return 0;

            double sum = values.Sum();
            double sumOfSquares = values.Sum(t => t*t);

            return (sumOfSquares - sum * sum / values.Length) / (values.Length - 1);
        }

        public static double StandardDeviation(double[] values)
        {
            double varianse = Variance(values);
            return Math.Sqrt(varianse);
        }
        
        public static double Map(double value, double rangeStart, double rangeEnd, double range,
            double mappedRangeStart, double mappedRangeEnd, double mappedRange, double scale)
        {
            if (value < rangeStart)
                value = rangeStart;
            else if (value > rangeEnd)
                value = rangeEnd;

            return (value - rangeStart) * scale + mappedRangeStart;
        }
    }
}

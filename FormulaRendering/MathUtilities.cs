using System;

namespace WallpaperGenerator.FormulaRendering
{
    public static class MathUtilities
    {
        public static double MathExpectation(double[] values)
        {
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
                sum += values[i];

            return sum/values.Length;
        }

        public static double Variance(double[] values)
        {
            if (values.Length == 1)
                return 0;

            double sum = 0;
            for (int i = 0; i < values.Length; i++)
                sum += values[i];

            double sumOfSquares = 0;
            for (int i = 0; i < values.Length; i++)
                sumOfSquares += values[i] * values[i];

            return (sumOfSquares - sum * sum / values.Length) / (values.Length - 1);
        }

        public static double StandardDeviation(double[] values)
        {
            double varianse = Variance(values);
            return Math.Sqrt(varianse);
        }

        public static double ThreeSigmas(double[] values)
        {
            double standartDeviation = StandardDeviation(values);
            return 3*standartDeviation;
        }

        public static double Map(double value, double rangeStart, double rangeEnd, double mappedRangeStart, double mappedRangeEnd)
        {
            if (double.IsNaN(value))
                return (mappedRangeEnd - mappedRangeStart) / 2;

            if (value < rangeStart)
                value = rangeStart;
            if (value > rangeEnd)
                value = rangeEnd;

            double range = rangeEnd - rangeStart;
            double mappedRange = mappedRangeEnd - mappedRangeStart;
            double scale = mappedRange / range;
            return (value - rangeStart) * scale + mappedRangeStart;
        }
    }
}

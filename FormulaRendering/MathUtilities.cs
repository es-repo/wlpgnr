using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.FormulaRendering
{
    public static class MathUtilities
    {
        public static double MathExpectation(IEnumerable<double> values)
        {
            int count = values.Count();
            return values.Sum(v => v / count);
        }

        public static double Variance(IEnumerable<double> values)
        {
            int count = values.Count();
            if (count == 1)
                return 0;

            double sum = values.Sum();
            double sumOfSquares = values.Sum(v => v * v);
            
            return (sumOfSquares - sum*sum/count)/(count - 1);
        }

        public static double StandardDeviation(IEnumerable<double> values)
        {
            double varianse = Variance(values);
            return Math.Sqrt(varianse);
        }

        public static double ThreeSigmas(IEnumerable<double> values)
        {
            double standartDeviation = StandardDeviation(values);
            return 3*standartDeviation;
        }

        public static double Map(double value, double rangeStart, double rangeEnd, double mappedRangeStart, double mappedRangeEnd)
        {            
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

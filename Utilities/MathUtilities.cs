using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WallpaperGenerator.Utilities
{
    public static class MathUtilities
    {
        public static double MathExpectation(float[] values, int threadsCount) 
        {
            double sum = Sum(values, null, threadsCount);
            return sum / values.Length;
        }

        public static double Variance(float[] values, int threadsCount)
        {
            if (values.Length == 1)
                return 0;

            double sum = Sum(values, null, threadsCount); 
            double mean = sum/values.Length;
            return Sum(values, v => (float) ((v - mean)*(v - mean)), threadsCount)/values.Length;
        }

        public static double StandardDeviation(float[] values, int threadsCount)
        {
            double varianse = Variance(values, threadsCount);
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

        public static IEnumerable<double> Normalize(IEnumerable<double> numbers)
        {
            double sum = numbers.Sum();
            return numbers.Select(n => n/sum);
        }

        public static float Sum(float[] values, Func<float, float> selector, int threadsCount)
        {
            if (threadsCount < 2 || values.Length <= threadsCount)
                return selector == null ? values.Sum() : values.Sum(selector);

            Task<float>[] tasks = new Task<float>[threadsCount];
            int chunk = (int)Math.Ceiling((double)values.Length/threadsCount);
            for (int i = 0; i < threadsCount; i++)
            {
                int start = i*chunk;
                int end = Math.Min(start + chunk, values.Length);
                tasks[i] = Task.Run(() => Sum(values, selector, start, end));
            }

            return tasks.Sum(t => t.Result);
        }

        private static float Sum(float[] values, Func<float, float> selector, int start, int end)
        {
            if (selector == null)
                return Sum(values, start, end);

            float s = 0;
            for (int i = start; i < end; i++)
                s += selector(values[i]);
            return s;
        }

        private static float Sum(float[] values, int start, int end)
        {
            float s = 0;
            for (int i = start; i < end; i++)
                s += values[i];
            return s;
        }
    }
}

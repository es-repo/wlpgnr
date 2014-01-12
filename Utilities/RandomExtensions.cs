using System;

namespace WallpaperGenerator.Utilities
{
    public static class RandomExtensions
    {
        public static T GetRandomBetweenTwo<T>(this Random random, T first, T second, double probabilityOfSecond)
        {
            double r = random.NextDouble();
            return r < probabilityOfSecond ? second : first;
        }

        public static T GetRandomBetweenThree<T>(this Random random, T first, T second, T third, 
            double probabilityOfSecond, double probabilityOfThird)
        {
            double r = random.NextDouble();
            return r < probabilityOfSecond
                ? second
                : r < probabilityOfSecond + probabilityOfThird
                    ? third
                    : first;                    
        }

        public static Bounds RandomlyShrinkBounds(this Random random, Bounds bounds, double minDiff)
        {
            double shrinkCoeficient = random.NextDouble();
            double newLowBound = bounds.Low * shrinkCoeficient;
            double newHighBound = bounds.High * shrinkCoeficient;
            return Math.Abs(newLowBound - newHighBound) < minDiff 
                ? bounds 
                : new Bounds(newLowBound, newHighBound);
        }

        public static double Next(this Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static T Next<T>(this Random random, Bounds<T> bounds) where T : IComparable
        {
            if (typeof (T) == typeof (int))
            {
                double low = (int) (object) bounds.Low;
                double high = (int)(object)bounds.High + 1;
                return (T)(object)(int)random.Next(low, high);
            }
            return (T)(object)random.Next((double)(object)bounds.Low, (double)(object)bounds.High);
        }
    }
}

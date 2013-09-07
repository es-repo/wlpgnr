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

        public static void RandomlyShrinkBounds(this Random random, ref int lowBound, ref int highBound)
        {
            if (lowBound > highBound)
                throw new ArgumentException("Low bound can't be bigger then high bound.", "lowBound");
            
            double shrinkCoeficient = random.NextDouble();
            
            int newLowBound = (int)(lowBound * shrinkCoeficient);
            int newHighBound = (int)(highBound * shrinkCoeficient);
            if (newLowBound == newHighBound)
                return;

            lowBound = newLowBound;
            highBound = newHighBound;
        }
    }
}

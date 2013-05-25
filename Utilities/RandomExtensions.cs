using System;

namespace WallpaperGenerator.Utilities
{
    public static class RandomExtensions
    {
        public static T GetRandomBetweenTwo<T>(this Random random, T first, T second, double secondOccurenceProbability)
        {
            double r = random.NextDouble();
            return r <= secondOccurenceProbability ? second : first;
        }

        public static T GetRandomBetweenThree<T>(this Random random, T first, T second, T third, 
            double secondOccurenceProbability, double thirdOccurenceProbability)
        {
            double r = random.NextDouble();
            return r <= secondOccurenceProbability
                ? second
                : r <= secondOccurenceProbability + thirdOccurenceProbability
                    ? third
                    : first;                    
        }

        public static void RandomlyShrinkBounds(this Random random, ref int lowBound, ref int highBound)
        {
            double boundShrinkCoeficient = random.NextDouble();
            int newLowBound = (int)(lowBound * boundShrinkCoeficient);
            int newHighBound = (int)(highBound * boundShrinkCoeficient);
            if (newLowBound > newHighBound)
            {
                int t = newLowBound;
                newLowBound = newHighBound;
                newHighBound = t;
            }
            else if (newLowBound == newHighBound)
            {
                return;
            }

            lowBound = newLowBound;
            highBound = newHighBound;
        }
    }
}

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
    }
}

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
    }
}

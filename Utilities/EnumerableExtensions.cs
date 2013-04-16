using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Repeat<T>(Func<int, T> func, int count)
        {
            for (int i = 0; i < count; i++)
                yield return func(i);
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source, Random random)
        {
            return source.OrderBy(item => random.Next());
        }

        public static T TakeRandom<T>(this IEnumerable<T> source, Random random)
        {
            int index = random.Next(source.Count()); 
            return source.Skip(index).First();
        }
    }
}

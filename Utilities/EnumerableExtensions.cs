using System;
using System.Collections.Generic;
using System.Linq;

namespace WallpaperGenerator.Utilities
{
    public static class EnumerableExtensions
    {        
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T i in items)
                action(i);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> items, T item)
        {
            foreach (T t in items) 
                yield return t;
            yield return item;
        }

        public static IEnumerable<T> Repeat<T>(Func<T> func, int? count = null)
        {
            return Repeat(i => func(), count);
        }

        public static IEnumerable<T> Repeat<T>(Func<int, T> func, int? count = null)
        {
            int i = 0;
            while (true)
            {
                if (count != null && i >= count)
                    break;

                yield return func(i);

                i++;
            }
        }

        public static IEnumerable<T> Repeat<T>(this IEnumerable<T> items, int? count = null)
        {
            if (!items.Any())
                throw new ArgumentException("Sequence is empty.", "items");

            int i = 0;
            while (true)
            {
                if (count != null && i >= count)
                    break;

                foreach (T item in items)
                    yield return item;

                i++;
            }
        }

        public static IEnumerable<R> SelectWithFolding<T, R>(this IEnumerable<T> source, Func<R, T, R> func, R initValue = default(R))
        {
            R previousValue = initValue; 
            foreach (T e in source)
            {
                R value = func(previousValue, e);
                yield return value;
                previousValue = value;
            }
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source, Random random)
        {
            return source.OrderBy(item => random.Next());
        }

        public static T TakeRandom<T>(this IEnumerable<T> source, Random random)
        {
            int index = (int)(random.NextDouble() * source.Count()); 
            return source.Skip(index).First();
        }

        public static T TakeRandom<T>(this IEnumerable<T> source, Random random, IEnumerable<double> elementProbabilities)
        {
            if (source.Count() != elementProbabilities.Count())
            { 
                throw new ArgumentException("Count of elements isn't equal with count of probabilties.");
            }

            elementProbabilities = MathUtilities.Normalize(elementProbabilities);
            double[] probabilisticRange = elementProbabilities.SelectWithFolding((p, c) => p + c, 0.0).ToArray();
            probabilisticRange[probabilisticRange.Length - 1] = 1;

            double r = random.NextDouble();
            int i = probabilisticRange.TakeWhile(p => r >= p).Count();
            return source.Skip(i).First();
        }
    }
}

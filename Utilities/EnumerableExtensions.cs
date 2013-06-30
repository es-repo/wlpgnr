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

        public static IEnumerable<R> SelectWithFolding<T, R>(this IEnumerable<T> source, Func<R, T, R> func, R initValue)
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
            int index = random.Next(source.Count()); 
            return source.Skip(index).First();
        }

        public static T TakeRandom<T>(this IEnumerable<T> source, Random random, IEnumerable<double> elementProbabilities)
        {
            if (source.Count() != elementProbabilities.Count())
            { 
                throw new ArgumentException("Count of elements isn't equal to count of probabilties.");
            }

            if (!elementProbabilities.Sum(p => p).Equals(1))
            {
                throw new ArgumentException("Sum of probabilties isn't equal to 1.");
            }

            IEnumerable<double> probabilisticRange = elementProbabilities.SelectWithFolding((p, c) => p + c, 0.0); 

            double r = random.NextDouble();
            int i = 0;
            foreach (double p in probabilisticRange)
            {
                if (r <= p)
                    break;
                i++;
            }
            return source.Skip(i).First();
        }
    }
}

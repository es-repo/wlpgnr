using System;

namespace WallpaperGenerator.Utilities
{
    public static class FuncUtilities
    {
        public static T Repeat<T>(Func<T> func, Func<T, bool> till, int maxRepeats)
        {
            if (maxRepeats < 1)
                throw new ArgumentException("Maximum of repeats should be greater then 0.", "maxRepeats");

            T result;
            do
            {
                result = func();
            } while (!till(result) && --maxRepeats > 0);
            return result;
        }
    }
}

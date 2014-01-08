using System;

namespace WallpaperGenerator.Utilities
{
    public class Bounds<T> where T : IComparable
    {
        public T Low { get; private set; }

        public T High { get; private set; }

        public Bounds(T low, T high)
        {
            if (low.CompareTo(high) > 0)
                throw new ArgumentException("Low bound can't be more then high bound", "low");
            Low = low;
            High = high;
        }
    }

    public class Bounds : Bounds<double>
    {
        public Bounds(double low, double high) 
            : base(low, high)
        {
        }
    }
}

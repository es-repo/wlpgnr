using System.Collections.Generic;

namespace WallpaperGenerator.Formulas
{
    public class Range
    {
        public double Start { get; private set; }
        public int Count { get; private set; }
        public double Interval { get; private set; }

        public Range(double start, int count)
            : this(start, 1, count)
        {
        }

        public Range(double start, double interval, int count)
        {
            Start = start;
            Count = count;
            Interval = interval;
        }

        public IEnumerable<double> Values
        {
            get
            {
                double v = Start; 
                for (int i = 0; i < Count; i++, v+= Interval)
                    yield return v;
            }
        }
    }
}

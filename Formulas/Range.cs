using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WallpaperGenerator.Formulas
{
    public class Range
    {
        public double Start { get; private set; }
        public double Step { get; private set; }
        public int Count { get; private set; }

        public Range(double start, int count)
            : this(start, 1, count)
        {
        }

        public Range(double start, double step, int count)
        {
            Start = start;
            Count = count;
            Step = step;
        }

        public IEnumerable<double> Values
        {
            get
            {
                double v = Start; 
                for (int i = 0; i < Count; i++, v+= Step)
                    yield return v;
            }
        }

        public override string ToString()
        {
            double[] rangeElements = new[] { Start, Step, Count };
            return string.Join(",", rangeElements.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray());
        }

        public static Range FromString(string value)
        {
            string[] rangeElements = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            double start = double.Parse(rangeElements[0]);
            double step = double.Parse(rangeElements[1]);
            int count = int.Parse(rangeElements[2]);
            return new Range(start, step, count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace WallpaperGenerator.Formulas
{
    public class Range
    {
        public double Start { get; set; }
        public double Step { get; set; }
        public int Count { get; set; }

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
            return ToString(false);
        }

        public string ToString(bool omitCount)
        {
            List<double> rangeElements = new List<double> { Start, Step };
            if (!omitCount)
            {
                rangeElements.Add(Count);
            }
            return string.Join(",", rangeElements.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray());
        }

        public static Range FromString(string value)
        {
            string[] rangeElements = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            double start = double.Parse(rangeElements[0]);
            double step = double.Parse(rangeElements[1]);
            int count = rangeElements.Length > 2 ? int.Parse(rangeElements[2]) : 0;
            return new Range(start, step, count);
        }

        public static Range CreateRandom(Random random, int rangeCount, int rangeLowBound, int rangeHighBound)
        {
            double start = Math.Round(random.NextDouble() * random.Next(rangeLowBound, rangeHighBound), 2);
            double end = Math.Round(random.NextDouble() * random.Next(rangeLowBound, rangeHighBound), 2);
            if (start > end)
            {
                double t = start;
                start = end;
                end = t;
            }
            else if (start.Equals(end))
            {
                start = rangeLowBound;
                end = rangeHighBound;
            }
            double step = Math.Round((end - start) / rangeCount, 4);
            return new Range(start, step, rangeCount);
        }
    }
}

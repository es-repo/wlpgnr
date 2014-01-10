using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.Formulas
{
    public class Range
    {
        public double Start { get; private set; }
        public double End { get; private set; }
        public double Step { get; private set; }
        public int Count { get; private set; }

        public Range(double start, double end)
            : this(start, end, 1.0)
        {
        }

        public Range(double start, double end, int count)
            : this(start, end, (end - start) / count)
        {
            Start = start;
            End = end;
            Count = count;
            Step = (End - Start) / Count;
        }

        public Range(double start, double end, double step)
        {
            Start = start;
            End = end;
            Step = step;
            Count = (int)((End - Start)/Step);
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool omitStep)
        {
            List<double> rangeElements = new List<double> { Start, End };
            if (!omitStep)
            {
                rangeElements.Add(Step);
            }
            return string.Join(",", rangeElements.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray());
        }

        public static Range FromString(string value)
        {
            string[] rangeElements = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            double start = double.Parse(rangeElements[0]);
            double end = double.Parse(rangeElements[1]);
            double step = rangeElements.Length > 2 ? double.Parse(rangeElements[2]) : 1.0;
            return new Range(start, end, step);
        }

        public static Range CreateRandom(Random random, int rangeCount, double rangeLowBound, double rangeHighBound)
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
           
            return new Range(start, end, rangeCount);
        }
    }
}
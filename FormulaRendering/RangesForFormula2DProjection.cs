using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public class RangesForFormula2DProjection
    {
        public int XCount { get; private set; }

        public int YCount { get; private set; }

        public Range[] Ranges { get; private set; }

        public int IterationsCount { get; private set; }

        public double[] IterationRangeScales { get; private set; }

        public RangesForFormula2DProjection(int xCount, int yCount, int iterationsCount, IEnumerable<Range> ranges, IEnumerable<double> iterationRangeScales)
        {
            XCount = xCount;
            YCount = yCount;
            Ranges = ranges.Select((r, i) => new Range(r.Start, r.Step, i%2 == 0 ? XCount : YCount)).ToArray();

            if (iterationsCount < 1)
                throw new ArgumentException("Iterations count should be greater then 0", "iterationsCount");

            IterationsCount = iterationsCount;
            IterationRangeScales = iterationRangeScales.ToArray();

            if (Ranges.Length != IterationRangeScales.Length)
                throw new ArgumentException("Number of ranges should be equal to number of iteration range scales.", "iterationRangeScales");
        }

        public override string ToString()
        {
            string[] countStrings = new [] { XCount.ToInvariantString(), YCount.ToInvariantString(), IterationsCount.ToInvariantString() };
            IEnumerable<string> rangeStrings = Ranges.Select(r => r.ToString(true));
            IEnumerable<string> scaleStrings = IterationRangeScales.Select(r => r.ToInvariantString());
            return string.Join(";", countStrings.Concat(rangeStrings).Concat(scaleStrings).ToArray());
        }

        public static RangesForFormula2DProjection FromString(string value)
        {
            string[] rangeStrings = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int xCount = int.Parse(rangeStrings[0]);
            int yCount = int.Parse(rangeStrings[1]);
            int iterationsCount = int.Parse(rangeStrings[2]);
            const int rangesStartIndex = 3;
            int rangesCount = (rangeStrings.Length - rangesStartIndex) / 2;
            IEnumerable<Range> ranges = rangeStrings.Skip(rangesStartIndex).Take(rangesCount).Select(Range.FromString);
            IEnumerable<double> scales = rangeStrings.Skip(rangesStartIndex + rangesCount).Select(double.Parse);
            return new RangesForFormula2DProjection(xCount, yCount, iterationsCount, ranges, scales);
        }

        public static RangesForFormula2DProjection CreateRandom(Random random, int variableCount,
            int xRangeCount, int yRangeCount, int iterationsCount, int rangeLowBound, int rangeHighBound)
        {
            random.RandomlyShrinkBounds(ref rangeLowBound, ref rangeHighBound);
            IEnumerable<Range> ranges = EnumerableExtensions.Repeat(
                i => Range.CreateRandom(random, i % 2 == 0 ? xRangeCount : yRangeCount, rangeLowBound, rangeHighBound), variableCount);
            IEnumerable<double> scales = EnumerableExtensions.Repeat(() => Math.Round(random.NextDouble() * 2 + 0.01, 2), variableCount);
            return new RangesForFormula2DProjection(xRangeCount, yRangeCount, iterationsCount, ranges, scales);
        }
    }
}

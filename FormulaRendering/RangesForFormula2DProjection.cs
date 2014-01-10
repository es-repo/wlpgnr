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

        public int IterationCount { get; private set; }

        public double IterationScale { get; private set; }

        public RangesForFormula2DProjection(int xCount, int yCount, IEnumerable<Range> ranges, int iterationCount, double iterationScale)
        {
            XCount = xCount;
            YCount = yCount;
            Ranges = ranges.Select((r, i) => new Range(r.Start, r.Step, i%2 == 0 ? XCount : YCount)).ToArray();

            if (iterationCount < 1)
                throw new ArgumentException("Iterations count should be greater then 0", "iterationCount");

            IterationCount = iterationCount;
            IterationScale = iterationScale;
        }

        public override string ToString()
        {
            string[] countStrings = { XCount.ToInvariantString(), YCount.ToInvariantString() };
            IEnumerable<string> rangeStrings = Ranges.Select(r => r.ToString(true));
            string[] iterationStrings = { IterationCount.ToInvariantString(), IterationScale.ToInvariantString() };
            return string.Join(";", countStrings.Concat(rangeStrings).Concat(iterationStrings).ToArray());
        }

        public static RangesForFormula2DProjection FromString(string value)
        {
            string[] rangeStrings = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int xCount = int.Parse(rangeStrings[0]);
            int yCount = int.Parse(rangeStrings[1]);
            int iterationCount = int.Parse(rangeStrings[rangeStrings.Length - 2]);
            double iterationScale = double.Parse(rangeStrings[rangeStrings.Length - 1]);
            const int rangesStartIndex = 2;
            int rangesEndIndex = rangeStrings.Length - 2;
            int rangesCount = rangesEndIndex - rangesStartIndex;
            IEnumerable<Range> ranges = rangeStrings.Skip(rangesStartIndex).Take(rangesCount).Select(Range.FromString);
            return new RangesForFormula2DProjection(xCount, yCount, ranges, iterationCount, iterationScale);
        }

        public static RangesForFormula2DProjection CreateRandom(Random random, int variableCount,
            int xRangeCount, int yRangeCount, int iterationsCount, Bounds rangeBounds)
        {
            rangeBounds = random.RandomlyShrinkBounds(rangeBounds, 1);
            IEnumerable<Range> ranges = EnumerableExtensions.Repeat(
                i => Range.CreateRandom(random, i % 2 == 0 ? xRangeCount : yRangeCount, rangeBounds.Low, rangeBounds.High), variableCount);
            double scale = Math.Round(1 + random.NextDouble() * 0.5, 2);
            return new RangesForFormula2DProjection(xRangeCount, yRangeCount, ranges, iterationsCount, scale);
        }
    }
}

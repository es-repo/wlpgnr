using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public class RangesForFormula2DProjection
    {
        public Size AreaSize { get; private set; }

        public Range[] Ranges { get; set; }

        public int IterationCount { get; private set; }

        public double IterationScale { get; private set; }

        public RangesForFormula2DProjection(Size areaSize, IEnumerable<Range> ranges, int iterationCount, double iterationScale)
        {
            AreaSize = areaSize;
            Ranges = ranges.Select((r, i) => new Range(r.Start, r.End, i % 2 == 0 ? AreaSize.Width : AreaSize.Height)).ToArray();

            if (iterationCount < 1)
                throw new ArgumentException("Iterations count should be greater then 0", "iterationCount");

            IterationCount = iterationCount;
            IterationScale = iterationScale;
        }

        public override string ToString()
        {
            IEnumerable<string> rangeStrings = Ranges.Select(r => r.ToString(true));
            return string.Join(";", rangeStrings.ToArray());
        }

        public static RangesForFormula2DProjection FromString(string value)
        {
            string[] rangeStrings = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            IEnumerable<Range> ranges = rangeStrings.Select(Range.FromString);
            return new RangesForFormula2DProjection(new Size(1, 1), ranges, 1, 1);
        }

        public static RangesForFormula2DProjection CreateRandom(Random random, int variableCount,
            Size areaSize, int iterationsCount, Bounds rangeBounds)
        {
            rangeBounds = random.RandomlyShrinkBounds(rangeBounds, 1);
            IEnumerable<Range> ranges = EnumerableExtensions.Repeat(
                i => Range.CreateRandom(random, i % 2 == 0 ? areaSize.Width : areaSize.Height, rangeBounds.Low, rangeBounds.High), variableCount);
            double scale = Math.Round(1 + random.NextDouble() * 0.5, 2);
            return new RangesForFormula2DProjection(areaSize, ranges, iterationsCount, scale);
        }

        public RangesForFormula2DProjection Clone(Size areaSize)
        {
            return new RangesForFormula2DProjection(areaSize, Ranges, IterationCount, IterationScale);
        }
    }
}
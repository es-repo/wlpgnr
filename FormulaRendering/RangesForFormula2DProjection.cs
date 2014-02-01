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

        public RangesForFormula2DProjection(Size areaSize, IEnumerable<Range> ranges)
        {
            AreaSize = areaSize;
            Ranges = ranges.Select((r, i) => new Range(r.Start, r.End, i % 2 == 0 ? AreaSize.Width : AreaSize.Height)).ToArray();
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
            return new RangesForFormula2DProjection(new Size(1, 1), ranges);
        }

        public static RangesForFormula2DProjection CreateRandom(Random random, int variableCount,
            Size areaSize, int iterationsCount, Bounds rangeBounds)
        {
            rangeBounds = random.RandomlyShrinkBounds(rangeBounds, 3);
            IEnumerable<Range> ranges = EnumerableExtensions.Repeat(
                i => Range.CreateRandom(random, i % 2 == 0 ? areaSize.Width : areaSize.Height, rangeBounds.Low, rangeBounds.High, 1), variableCount);
            return new RangesForFormula2DProjection(areaSize, ranges);
        }

        public RangesForFormula2DProjection Clone(Size areaSize)
        {
            return new RangesForFormula2DProjection(areaSize, Ranges);
        }
    }
}
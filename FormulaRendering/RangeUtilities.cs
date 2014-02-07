using System;
using System.Collections.Generic;
using System.Linq;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.FormulaRendering
{
    public static class RangesForFormula2DProjection
    {
        public static string ToString(Range[] ranges)
        {
            IEnumerable<string> rangeStrings = ranges.Select(r => r.ToString(true));
            return string.Join(";", rangeStrings.ToArray());
        }

        public static Range[] FromString(string value)
        {
            string[] rangeStrings = value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return rangeStrings.Select(Range.FromString).ToArray();
        }

        public static Range[] CreateRandom(Random random, int variableCount, int iterationsCount, Bounds rangeBounds)
        {
            rangeBounds = random.RandomlyShrinkBounds(rangeBounds, 3);
            return EnumerableExtensions.Repeat(i => Range.CreateRandom(random, 1, rangeBounds.Low, rangeBounds.High, 1),variableCount).ToArray();
        }
    }
}
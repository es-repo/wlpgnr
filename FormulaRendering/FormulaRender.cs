using System;
using System.Collections.Generic;
using System.Linq;  
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        public static RenderedFormulaImage Render(FormulaTree formulaTree, Range[] variableValuesRanges, ColorTransformation colorTransformation, 
            int width, int height)
        {
            double[] formulaEvaluatedValues = formulaTree.EvaluateRangesIn2DProjection(variableValuesRanges).ToArray();
            IEnumerable<Rgb> data = MapToRgb(formulaEvaluatedValues, colorTransformation);
            return new RenderedFormulaImage(data.ToArray(), width, height);
        }

        private static IEnumerable<Rgb> MapToRgb(double[] values, ColorTransformation colorTransformation)
        {
            IEnumerable<byte> redChannel = MapToColorChannel(values, colorTransformation.RedChannelTransformation);
            IEnumerable<byte> greenChannel = MapToColorChannel(values, colorTransformation.GreenChannelTransformation);
            IEnumerable<byte> blueChannel = MapToColorChannel(values, colorTransformation.BlueChannelTransformation);

            IEnumerator<byte> greenChannelEnumerator = greenChannel.GetEnumerator();
            IEnumerator<byte> blueChannelEnumerator = blueChannel.GetEnumerator();
            foreach (byte r in redChannel)
            {
                greenChannelEnumerator.MoveNext();
                byte g = greenChannelEnumerator.Current;

                blueChannelEnumerator.MoveNext();
                byte b = blueChannelEnumerator.Current;

                yield return new Rgb(r, g, b);
            }
        }

        private static IEnumerable<byte> MapToColorChannel(IEnumerable<double> values, ColorChannelTransformation colorChannelTransformation)
        {
            double[] channelValues = TransformChannelValues(values, colorChannelTransformation.TransformationFunction).ToArray();
            
            IEnumerable<double> significantValues = GetSignificantValues(channelValues);

            double mathExpectation = MathUtilities.MathExpectation(significantValues);
            double standardDeviation = MathUtilities.StandardDeviation(significantValues);
            double limit = standardDeviation * (1 + 2 * colorChannelTransformation.DispersionCoefficient);
            if (double.IsNegativeInfinity(limit))
                limit = double.MinValue;
            if (double.IsPositiveInfinity(limit))
                limit = double.MaxValue;

            double rangeStart = mathExpectation - limit;
            double rangeEnd = mathExpectation + limit;
            if (rangeStart > rangeEnd)
            {
                double tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }

            return channelValues.Select(v => (byte)MathUtilities.Map(v, rangeStart, rangeEnd, 0, 255));
        }

        private static IEnumerable<double> TransformChannelValues(IEnumerable<double> values, Func<double, double> channelTransformingFunction)
        {
            return values.Select(channelTransformingFunction);
        }

        private static IEnumerable<double> GetSignificantValues(IEnumerable<double> values)
        {
            const double factor = 1e175;
            const double lowBound = double.MinValue * factor;
            const double highBound = double.MaxValue / factor;
            return values.Where(v => !double.IsNaN(v) && (v > lowBound) && (v < highBound));
        }
    }
}

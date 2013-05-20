using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;  
using WallpaperGenerator.Formulas;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        public static RenderedFormulaImage Render(FormulaTree formulaTree, Range[] variableValuesRanges, ColorTransformation colorTransformation, 
            int width, int height)
        {
            Stopwatch evaluationStopwatch = new Stopwatch();
            evaluationStopwatch.Start();
            double[] formulaEvaluatedValues = formulaTree.EvaluateRangesIn2DProjection(variableValuesRanges);
            evaluationStopwatch.Stop();

            //double r;
            //double x = 1;
            //double y = 2;
            //double z = 3;
            //double w = 4;
            //double x1 = -1;
            //double y1 = -2;
            //double z1 = -3;
            //double w1 = -4;
            //Stopwatch stopwatch2 = new Stopwatch();
            //stopwatch2.Start();
            //for (int i = 0; i < variableValuesRanges[0].Count; i++)
            //    for (int j = 0; j < variableValuesRanges[1].Count; j++)
            //    {
            //        r = Math.Sqrt((Math.Sin(x) * Math.Sin(y) + Math.Sin(z) * Math.Sin(w)) * (Math.Sin(x1) * Math.Sin(y1) + Math.Sin(z1) * Math.Sin(w1)));
            //        x += 0.1;
            //        y += 0.1;
            //        z += 0.1;
            //        w += 0.1;
            //        x1 += 0.3;
            //        y1 += 0.3;
            //        z1 += 0.3;
            //        w1 += 0.3;
            //    }
            //stopwatch2.Stop();

            Stopwatch mapToRgbStopwatch = new Stopwatch();
            mapToRgbStopwatch.Start();  
            Rgb[] data = MapToRgb(formulaEvaluatedValues, colorTransformation);
            mapToRgbStopwatch.Stop();
            return new RenderedFormulaImage(data, width, height);
        }

        private static Rgb[] MapToRgb(double[] values, ColorTransformation colorTransformation)
        {
            byte[] redChannel = MapToColorChannel(values, colorTransformation.RedChannelTransformation);
            byte[] greenChannel = MapToColorChannel(values, colorTransformation.GreenChannelTransformation);
            byte[] blueChannel = MapToColorChannel(values, colorTransformation.BlueChannelTransformation);

            Rgb[] colors = new Rgb[redChannel.Length];
            for (int i = 0; i < redChannel.Length; i++)
            {
                colors[i] = new Rgb(redChannel[i], greenChannel[i], blueChannel[i]);
            }
            return colors;
        }

        private static byte[] MapToColorChannel(double[] values, ColorChannelTransformation colorChannelTransformation)
        {
            double[] channelValues = TransformChannelValues(values, colorChannelTransformation.TransformationFunction);
            
            double[] significantValues = GetSignificantValues(channelValues);

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

            byte[] bytes = new byte[channelValues.Length];
            for (int i = 0; i < channelValues.Length; i++)
            {
                bytes[i] = (byte) MathUtilities.Map(channelValues[i], rangeStart, rangeEnd, 0, 255);
            }

            return bytes;
        }

        private static double[] TransformChannelValues(double[] values, Func<double, double> channelTransformingFunction)
        {
            double[] transformedValues = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                transformedValues[i] = channelTransformingFunction(values[i]);
            }
            return transformedValues;
        }

        private static double[] GetSignificantValues(IEnumerable<double> values)
        {
            const double factor = 1e175;
            const double lowBound = double.MinValue * factor;
            const double highBound = double.MaxValue / factor;
            return values.Where(v => !double.IsNaN(v) && (v > lowBound) && (v < highBound)).ToArray();
        }
    }
}

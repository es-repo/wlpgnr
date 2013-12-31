using System;
using System.Diagnostics;
using System.Linq;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        public static RenderedFormulaImage Render(FormulaTree formulaTree, RangesForFormula2DProjection rangesForFormula2DProjection, ColorTransformation colorTransformation)
        {
            Stopwatch evaluationStopwatch = new Stopwatch();
            evaluationStopwatch.Start();

            double[] formulaEvaluatedValues = new double[rangesForFormula2DProjection.XCount * rangesForFormula2DProjection.YCount];
            using (ProgressReporter.CreateScope(0.95))
            {
                using (ProgressReporter.CreateScope(rangesForFormula2DProjection.IterationCount))
                {
                    Range[] ranges = rangesForFormula2DProjection.Ranges.ToArray();
                    for (int i = 0; i < rangesForFormula2DProjection.IterationCount; i++)
                    {
                        double[] values = formulaTree.EvaluateRangesIn2DProjection(ranges, rangesForFormula2DProjection.XCount, rangesForFormula2DProjection.YCount);
                        for (int j = 0; j < values.Length; j++)
                        {
                            formulaEvaluatedValues[j] += values[j];
                        }

                        ranges = ranges.Select(r => new Range(r.Start*rangesForFormula2DProjection.IterationScale,
                                                r.Step*rangesForFormula2DProjection.IterationScale,
                                                r.Count)).ToArray();
                    }
                    ProgressReporter.Increase();
                }
            }
            
            evaluationStopwatch.Stop();

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
            //double[] arr = new double[Ranges[0].Count*Ranges[1].Count];
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    arr[i] = Math.Sqrt((Math.Sin(x) * Math.Sin(y) + Math.Sin(z) * Math.Sin(w)) * (Math.Sin(x1) * Math.Sin(y1) + Math.Sin(z1) * Math.Sin(w1)));
            //    x += 0.1;
            //    y += 0.1;
            //    z += 0.1;
            //    w += 0.1;
            //    x1 += 0.3;
            //    y1 += 0.3;
            //    z1 += 0.3;
            //    w1 += 0.3;
            //}
            //stopwatch2.Stop();

            Stopwatch mapToRgbStopwatch = new Stopwatch();
            mapToRgbStopwatch.Start();
            ProgressReporter.CreateScope(3, 0.05);
            byte[] redChannel = MapToColorChannel(formulaEvaluatedValues, colorTransformation.RedChannelTransformation);
            ProgressReporter.Increase();
            byte[] greenChannel = MapToColorChannel(formulaEvaluatedValues, colorTransformation.GreenChannelTransformation);
            ProgressReporter.Increase();
            byte[] blueChannel = MapToColorChannel(formulaEvaluatedValues, colorTransformation.BlueChannelTransformation);
            ProgressReporter.Complete();

            mapToRgbStopwatch.Stop();
            return new RenderedFormulaImage(redChannel, greenChannel, blueChannel, rangesForFormula2DProjection.XCount, rangesForFormula2DProjection.YCount);
        }

        private static byte[] MapToColorChannel(double[] values, ColorChannelTransformation colorChannelTransformation)
        {
            double[] channelValues = TransformChannelValues(values, colorChannelTransformation.TransformationFunction);

            const double factor = 1e175;
            const double lowBound = double.MinValue * factor;
            const double highBound = double.MaxValue / factor;
            LimitValue(channelValues, lowBound, highBound);

            double mathExpectation = MathUtilities.MathExpectation(channelValues);
            double standardDeviation = MathUtilities.StandardDeviation(channelValues);
            double limit = standardDeviation * (1 + 2 * colorChannelTransformation.DispersionCoefficient);
            if (double.IsNegativeInfinity(limit))
                limit = lowBound;
            if (double.IsPositiveInfinity(limit))
                limit = highBound;

            double rangeStart = mathExpectation - limit;
            double rangeEnd = mathExpectation + limit;
            if (rangeStart > rangeEnd)
            {
                double tmp = rangeStart;
                rangeStart = rangeEnd;
                rangeEnd = tmp;
            }
            double range = rangeEnd - rangeStart;
            double scale = 255/range;

            byte[] bytes = new byte[channelValues.Length];
            for (int i = 0; i < channelValues.Length; i++)
            {
                if (channelValues[i] < rangeStart)
                    channelValues[i] = rangeStart;
                else if (channelValues[i] > rangeEnd)
                    channelValues[i] = rangeEnd;

                bytes[i] = (byte)((channelValues[i] - rangeStart) * scale);
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

        private static void LimitValue(double[] values, double lowBound, double highBound)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > lowBound && values[i] < highBound)
                    continue;

                values[i] = values[i] < lowBound
                    ? lowBound
                    : values[i] > highBound
                        ? highBound
                        : double.IsNaN(values[i])
                            ? 0
                            : values[i];
            }
        }
    }
}

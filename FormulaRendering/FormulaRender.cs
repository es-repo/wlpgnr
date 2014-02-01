using System;
using System.Diagnostics;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        private static void EvaluateFormula(FormulaTree formulaTree, RangesForFormula2DProjection rangesForFormula2DProjection, double[] evaluatedValuesBuffer)
        {
            Stopwatch evaluationStopwatch = new Stopwatch();
            evaluationStopwatch.Start();

            if (evaluatedValuesBuffer.Length != rangesForFormula2DProjection.AreaSize.Width * rangesForFormula2DProjection.AreaSize.Height)
                throw new ArgumentException("Result buffer size isn't equal to ranges area size.", "evaluatedValuesBuffer");

            formulaTree.EvaluateRangesIn2DProjection( rangesForFormula2DProjection.Ranges, rangesForFormula2DProjection.AreaSize.Width, rangesForFormula2DProjection.AreaSize.Height, evaluatedValuesBuffer);

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

            evaluationStopwatch.Stop();
        }

        private static void Render(ColorTransformation colorTransformation, FormulaRenderResult formulaRenderResult)
        {
            Stopwatch mapToRgbStopwatch = new Stopwatch();
            mapToRgbStopwatch.Start();
            ProgressReporter.CreateScope(3);
            MapToColorChannel(colorTransformation.RedChannelTransformation, formulaRenderResult.EvaluatedValuesBuffer, formulaRenderResult.ColorTranformedValuesBuffer, formulaRenderResult.RedChannel);
            ProgressReporter.Increase();
            MapToColorChannel(colorTransformation.GreenChannelTransformation, formulaRenderResult.EvaluatedValuesBuffer, formulaRenderResult.ColorTranformedValuesBuffer, formulaRenderResult.GreenChannel);
            ProgressReporter.Increase();
            MapToColorChannel(colorTransformation.BlueChannelTransformation, formulaRenderResult.EvaluatedValuesBuffer, formulaRenderResult.ColorTranformedValuesBuffer, formulaRenderResult.BlueChannel);
            ProgressReporter.Complete();
            mapToRgbStopwatch.Stop();
        }

        public static void Render(FormulaTree formulaTree, RangesForFormula2DProjection rangesForFormula2DProjection, ColorTransformation colorTransformation,
            bool reevaluateValues, FormulaRenderResult formulaRenderResult)
        {
            using (ProgressReporter.CreateScope())
            {
                double evaluationSpan = 0;
                if (reevaluateValues)
                {
                    evaluationSpan = 0.93;
                    using (ProgressReporter.CreateScope(evaluationSpan))
                    {
                        EvaluateFormula(formulaTree, rangesForFormula2DProjection, formulaRenderResult.EvaluatedValuesBuffer);
                    }
                }

                using (ProgressReporter.CreateScope(1 - evaluationSpan))
                {
                    Render(colorTransformation, formulaRenderResult);
                }
            }
        }

        private static void MapToColorChannel(ColorChannelTransformation colorChannelTransformation, double[] values, double[] channelValuesBuffer, byte[] channelBuffer)
        {
            using (ProgressReporter.CreateScope())
            {
                ProgressReporter.CreateScope(0.16);
                TransformChannelValues(values, colorChannelTransformation.TransformationFunction, channelValuesBuffer);
                ProgressReporter.Complete();

                ProgressReporter.CreateScope(0.06);
                const double factor = 1e175;
                const double lowBound = double.MinValue*factor;
                const double highBound = double.MaxValue/factor;
                LimitValue(channelValuesBuffer, lowBound, highBound);
                ProgressReporter.Complete();

                ProgressReporter.CreateScope(0.21);
                double mathExpectation = MathUtilities.MathExpectation(channelValuesBuffer);
                ProgressReporter.Complete();

                ProgressReporter.CreateScope(0.48);
                double standardDeviation = MathUtilities.StandardDeviation(channelValuesBuffer);
                ProgressReporter.Complete();

                ProgressReporter.CreateScope(0.09);
                double limit = standardDeviation*(1 + 2*colorChannelTransformation.DispersionCoefficient);
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

                for (int i = 0; i < channelBuffer.Length; i++)
                {
                    if (channelValuesBuffer[i] < rangeStart)
                        channelValuesBuffer[i] = rangeStart;
                    else if (channelValuesBuffer[i] > rangeEnd)
                        channelValuesBuffer[i] = rangeEnd;

                    channelBuffer[i] = (byte)((channelValuesBuffer[i] - rangeStart) * scale);
                }
                ProgressReporter.Complete();
            }
        }

        private static void TransformChannelValues(double[] values, Func<double, double> channelTransformingFunction, double[] transformedValuesBuffer)
        {
            if (transformedValuesBuffer.Length != values.Length)
                throw new ArgumentException("Transformed values buffer size isn't equal to values array syze.", "transformedValuesBuffer");

            for (int i = 0; i < values.Length; i++)
            {
                transformedValuesBuffer[i] = channelTransformingFunction(values[i]);
            }
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

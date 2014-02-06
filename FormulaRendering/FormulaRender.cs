using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.FormulaRendering
{
    public static class FormulaRender
    {
        private static void EvaluateFormula(FormulaTree formulaTree, Range[] ranges, Size areaSize, int threadsCount, float[] evaluatedValuesBuffer)
        {
            Stopwatch evaluationStopwatch = new Stopwatch();
            evaluationStopwatch.Start();

            if (evaluatedValuesBuffer.Length != areaSize.Square)
                throw new ArgumentException("Result buffer size isn't equal to ranges area size.", "evaluatedValuesBuffer");

            int xCount = areaSize.Width;
            int yCount = areaSize.Height;

            if (threadsCount < 1)
                threadsCount = 1;

            ranges = ranges.Select((r, i) => new Range(r.Start, r.End, i % 2 == 0 ? areaSize.Width : areaSize.Height)).ToArray();
            const int progressSteps = 100;
            using (ProgressReporter.CreateScope(progressSteps))
            {
                int steps = 0;
                double lastProgress = 0;
                double progress = 0;
                ProgressObserver progressObserver = new ProgressObserver(p => progress = p.Progress);
                
                Task[] tasks = new Task[threadsCount];
                int yStepCount = yCount/threadsCount;
                for (int i = 0; i < threadsCount; i++)
                {
                    int li = i;
                    tasks[i] = Task.Run(() =>
                    {
                        FormulaTree ft = li == 0 ? formulaTree : FormulaTreeSerializer.Deserialize(FormulaTreeSerializer.Serialize(formulaTree));
                        int yStart = li * yStepCount;
                        ProgressReporter.Subscribe(progressObserver);
                        ft.EvaluateRangesIn2DProjection(ranges, xCount, yStart, yStepCount,
                            evaluatedValuesBuffer);
                    });
                }

                while (tasks.Any(t => t.Status != TaskStatus.RanToCompletion && t.Status != TaskStatus.Faulted && t.Status != TaskStatus.Canceled))
                {
                    Task.WaitAny(tasks, 100);
                    double progressCopy = progress;
                    int inc = (int)((progressCopy - lastProgress) * progressSteps);
                    if (inc > 0)
                    {
                        for (int i = 0; i < inc && steps < progressSteps; i++)
                        {
                            ProgressReporter.Increase();
                            steps++;
                        }
                        lastProgress = progress;
                    }
                }
                Task.WaitAll();
            }

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

        private static void Render(ColorTransformation colorTransformation, FormulaRenderResult formulaRenderResult, int threadsCount)
        {
            Stopwatch mapToRgbStopwatch = new Stopwatch();
            mapToRgbStopwatch.Start();
            ProgressReporter.CreateScope(3);
            MapToColorChannel(colorTransformation.RedChannelTransformation, formulaRenderResult.EvaluatedValuesBuffer, threadsCount, formulaRenderResult.ColorTranformedValuesBuffer, formulaRenderResult.RedChannel);
            ProgressReporter.Increase();
            MapToColorChannel(colorTransformation.GreenChannelTransformation, formulaRenderResult.EvaluatedValuesBuffer, threadsCount, formulaRenderResult.ColorTranformedValuesBuffer, formulaRenderResult.GreenChannel);
            ProgressReporter.Increase();
            MapToColorChannel(colorTransformation.BlueChannelTransformation, formulaRenderResult.EvaluatedValuesBuffer, threadsCount, formulaRenderResult.ColorTranformedValuesBuffer, formulaRenderResult.BlueChannel);
            ProgressReporter.Complete();
            mapToRgbStopwatch.Stop();
        }

        public static void Render(FormulaTree formulaTree, Range[] ranges, Size imageSize, ColorTransformation colorTransformation,
            bool reevaluateValues, int threadsCount, FormulaRenderResult formulaRenderResult)
        {
            using (ProgressReporter.CreateScope())
            {
                double evaluationSpan = 0;
                if (reevaluateValues)
                {
                    evaluationSpan = 0.93;
                    using (ProgressReporter.CreateScope(evaluationSpan))
                    {
                        EvaluateFormula(formulaTree, ranges, imageSize, threadsCount, formulaRenderResult.EvaluatedValuesBuffer);
                    }
                }

                using (ProgressReporter.CreateScope(1 - evaluationSpan))
                {
                    Render(colorTransformation, formulaRenderResult, threadsCount);
                }
            }
        }

        private static void MapToColorChannel(ColorChannelTransformation colorChannelTransformation, float[] values, int threadsCount, float[] channelValuesBuffer, byte[] channelBuffer)
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
                double mathExpectation = MathUtilities.MathExpectation(channelValuesBuffer, threadsCount);
                ProgressReporter.Complete();

                ProgressReporter.CreateScope(0.48);
                double standardDeviation = MathUtilities.StandardDeviation(channelValuesBuffer, threadsCount);
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
                        channelValuesBuffer[i] = (float)rangeStart;
                    else if (channelValuesBuffer[i] > rangeEnd)
                        channelValuesBuffer[i] = (float)rangeEnd;

                    channelBuffer[i] = (byte)((channelValuesBuffer[i] - rangeStart) * scale);
                }
                ProgressReporter.Complete();
            }
        }

        private static void TransformChannelValues(float[] values, Func<double, double> channelTransformingFunction, float[] transformedValuesBuffer)
        {
            if (transformedValuesBuffer.Length != values.Length)
                throw new ArgumentException("Transformed values buffer size isn't equal to values array syze.", "transformedValuesBuffer");

            for (int i = 0; i < values.Length; i++)
            {
                transformedValuesBuffer[i] = (float)channelTransformingFunction(values[i]);
            }
        }

        private static void LimitValue(float[] values, double lowBound, double highBound)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > lowBound && values[i] < highBound)
                    continue;

                double v = values[i] < lowBound
                    ? lowBound
                    : values[i] > highBound
                        ? highBound
                        : double.IsNaN(values[i])
                            ? 0
                            : values[i];
                values[i] = (float)v;
            }
        }
    }
}

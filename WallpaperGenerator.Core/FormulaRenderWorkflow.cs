using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderWorkflow
    {
        private readonly Random _random = new Random();
        private double[] _lastEvaluatedFormulaValues;

        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }
        public FormulaRenderArguments FormulaRenderArguments { get; private set; }

        public bool IsImageReady
        {
            get { return _lastEvaluatedFormulaValues != null; }
        }

        public FormulaRenderWorkflow(int imageWidth, int imageHeight)
        {
            ImageWidth = imageWidth;
            ImageHeight = imageHeight;

            FormulaRenderArguments = FormulaRenderArguments.FromString(
@"720;1280;-5.35,6.11;-21.74,-0.21;2.98,20.68;-0.56,18.97;-2.6,1.76;-7.25,0.25;-5.88,3.87;-4.73,1.87;1;1.28
0,0,0,0;-0.93,0.5,-1.24,0.02;0.04,-0.92,0.47,0.05
Sum Sin Cbrt Sub Tanh Ln Cbrt Sin Sin Atan Ln Atan Ln x7 Atan Ln Sin Atan Ln Tanh Ln Sin Sum x3 x7 Atan Ln Sum Sum Cbrt Sub x1 Sum Cbrt Tanh Ln Sum Sin x0 Sub x2 x6 Sum Sin Sub x7 Sin Sub x7 x2 x3 Sub Sum Atan Ln Sum Tanh Ln Sum x6 x4 Sin Sin Tanh Ln x0 Tanh Ln Cbrt Cbrt Sum Sum x4 x5 Sum x3 x2 Sin Sin Atan Ln Sub Sub Sub x1 x1 Sum x4 x5 x6 Atan Ln Tanh Ln Atan Ln Sub -7.47 Sin Atan Ln x0");       
        }

        public Task<FormulaRenderArguments> GenerateFormulaRenderArgumentsAsync()
        {
            _lastEvaluatedFormulaValues = null;
            return Task.Run(() => FormulaRenderArguments = GenerateRandomFormulaRenderArguments());
        }

        public FormulaRenderArguments ChangeColors()
        {
            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            return FormulaRenderArguments = new FormulaRenderArguments(FormulaRenderArguments.FormulaTree, FormulaRenderArguments.Ranges, colorTransformation);
        }

        public FormulaRenderArguments TransformRanges()
        {
            _lastEvaluatedFormulaValues = null;
            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(FormulaRenderArguments.FormulaTree.Variables.Length);
            return FormulaRenderArguments = new FormulaRenderArguments(
                FormulaRenderArguments.FormulaTree,
                ranges,
                FormulaRenderArguments.ColorTransformation);
        }

        private FormulaRenderArguments GenerateRandomFormulaRenderArguments()
        {
            FormulaTree formulaTree = CreateRandomFormulaTree();
            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length);
            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            return new FormulaRenderArguments(formulaTree, ranges, colorTransformation);
        }

        private RangesForFormula2DProjection CreateRandomVariableValuesRangesFor2DProjection(int variablesCount)
        {
            return RangesForFormula2DProjection.CreateRandom(_random, variablesCount, ImageWidth, ImageHeight, 1, FormulaRenderConfiguration.RangeBounds);
        }

        private ColorTransformation CreateRandomColorTransformation()
        {
            return ColorTransformation.CreateRandomPolynomialColorTransformation(_random,
                FormulaRenderConfiguration.ColorChannelPolinomialTransformationCoefficientBounds,
                FormulaRenderConfiguration.ColorChannelZeroProbabilty);
        }

        private FormulaTree CreateRandomFormulaTree()
        {
            int dimensionsCount = _random.Next(FormulaRenderConfiguration.DimensionCountBounds);
            int minimalDepth = _random.Next(FormulaRenderConfiguration.MinimalDepthBounds);
            double constantProbability = _random.Next(FormulaRenderConfiguration.ConstantProbabilityBounds);
            double leafProbability = _random.Next(FormulaRenderConfiguration.LeafProbabilityBounds);

            Operator[] operators = { OperatorsLibrary.Sum, OperatorsLibrary.Sub, OperatorsLibrary.Ln, OperatorsLibrary.Sin, 
                                       OperatorsLibrary.Max, OperatorsLibrary.Mul, OperatorsLibrary.Cbrt, OperatorsLibrary.Pow3 };
            IDictionary<Operator, double> operatorAndProbabilityMap = operators.ToDictionary(op => op, op => 0.5);

            Func<double> createConst = () =>
            {
                double c = _random.Next(FormulaRenderConfiguration.ConstantBounds);
                return Math.Abs(c - 0) < 0.01 ? 0.01 : c;
            };

            return FormulaTreeGenerator.Generate(operatorAndProbabilityMap, createConst, dimensionsCount, minimalDepth, _random, leafProbability, constantProbability);
        }

        public async Task<FormulaRenderResult> RenderFormulaAsync(ProgressObserver progressObserver)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double evaluationProgressSpan = 0;
            if (_lastEvaluatedFormulaValues == null)
            {
                evaluationProgressSpan = 0.97;
                _lastEvaluatedFormulaValues = await EvaluateFormulaAsync(FormulaRenderArguments, evaluationProgressSpan, progressObserver);
            }

            RenderedFormulaImage renderedFormulaImage = await RenderFormulaAsync(_lastEvaluatedFormulaValues, FormulaRenderArguments.WidthInPixels,
                FormulaRenderArguments.HeightInPixels, FormulaRenderArguments.ColorTransformation,
                1 - evaluationProgressSpan, evaluationProgressSpan, progressObserver);

            stopwatch.Stop();
            return new FormulaRenderResult(renderedFormulaImage, stopwatch.Elapsed);
        }

        private static Task<double[]> EvaluateFormulaAsync(FormulaRenderArguments formulaRenderingArguments, double progressSpan, ProgressObserver progressObserver)
        {
            return Task.Run(() =>
            {
                if (progressObserver != null)
                    ProgressReporter.Subscribe(progressObserver);
                using (ProgressReporter.CreateScope(progressSpan))
                    return FormulaRender.EvaluateFormula(formulaRenderingArguments.FormulaTree, formulaRenderingArguments.Ranges);
            });
        }

        private static Task<RenderedFormulaImage> RenderFormulaAsync(double[] evaluatedFormulaValues, int widthInPixels, int heightInPixels, ColorTransformation colorTransformation,
            double progressSpan, double initProgress, ProgressObserver progressObserver)
        {
            return Task.Run(() =>
            {
                if (progressObserver != null)
                    ProgressReporter.Subscribe(progressObserver);
                using (ProgressReporter.CreateScope(progressSpan, initProgress))
                    return FormulaRender.Render(evaluatedFormulaValues, widthInPixels, heightInPixels, colorTransformation);
            });
        }
    }
}

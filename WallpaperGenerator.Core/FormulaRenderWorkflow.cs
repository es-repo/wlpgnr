using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderWorkflow
    {
        private readonly Random _random = new Random();
        private FormulaRenderArguments _formulaRenderArguments;
        private double[] _lastEvaluatedFormulaValues;

        public FormulaRenderArgumentsGenerationParams GenerationParams { get; set; }
        
        public FormulaRenderArguments FormulaRenderArguments
        {
            get { return _formulaRenderArguments; }
            set 
            {
                if (_formulaRenderArguments == null || _formulaRenderArguments.ToString() != value.ToString())
                {
                    _formulaRenderArguments = value;
                    _lastEvaluatedFormulaValues = null;
                }
            }
        }

        public bool IsImageReady
        {
            get { return _lastEvaluatedFormulaValues != null; }
        }

        public FormulaRenderWorkflow(FormulaRenderArgumentsGenerationParams generationParams)
            : this(generationParams, new Random())
        {
        }

        public FormulaRenderWorkflow(FormulaRenderArgumentsGenerationParams generationParams, Random random)
        {
            GenerationParams = generationParams;
            _random = random;

            FormulaRenderArguments = FormulaRenderArguments.FromString(
@"720;1280;-5.35,6.11;-21.74,-0.21;2.98,20.68;-0.56,18.97;-2.6,1.76;-7.25,0.25;-5.88,3.87;-4.73,1.87;1;1.28
0,0,0,0;-0.93,0.5,-1.24,0.02;0.04,-0.92,0.47,0.05
Sum Sin Cbrt Sub Tanh Ln Cbrt Sin Sin Atan Ln Atan Ln x7 Atan Ln Sin Atan Ln Tanh Ln Sin Sum x3 x7 Atan Ln Sum Sum Cbrt Sub x1 Sum Cbrt Tanh Ln Sum Sin x0 Sub x2 x6 Sum Sin Sub x7 Sin Sub x7 x2 x3 Sub Sum Atan Ln Sum Tanh Ln Sum x6 x4 Sin Sin Tanh Ln x0 Tanh Ln Cbrt Cbrt Sum Sum x4 x5 Sum x3 x2 Sin Sin Atan Ln Sub Sub Sub x1 x1 Sum x4 x5 x6 Atan Ln Tanh Ln Atan Ln Sub -7.47 Sin Atan Ln x0");       
        }

        public Task<FormulaRenderArguments> GenerateFormulaRenderArgumentsAsync()
        {
            return Task.Run(() => GenerateFormulaRenderArguments());
        }

        public FormulaRenderArguments GenerateFormulaRenderArguments()
        {
            FormulaTree formulaTree = CreateRandomFormulaTree();
            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length);
            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            return FormulaRenderArguments = new FormulaRenderArguments(formulaTree, ranges, colorTransformation);
        }

        public FormulaRenderArguments ChangeColors()
        {
            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            return _formulaRenderArguments = new FormulaRenderArguments(FormulaRenderArguments.FormulaTree, FormulaRenderArguments.Ranges, colorTransformation);
        }

        public FormulaRenderArguments TransformRanges()
        {
            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(FormulaRenderArguments.FormulaTree.Variables.Length);
            return FormulaRenderArguments = new FormulaRenderArguments(
                FormulaRenderArguments.FormulaTree,
                ranges,
                FormulaRenderArguments.ColorTransformation);
        }

        private RangesForFormula2DProjection CreateRandomVariableValuesRangesFor2DProjection(int variablesCount)
        {
            return RangesForFormula2DProjection.CreateRandom(_random, variablesCount, GenerationParams.WidthInPixels, GenerationParams.HeightInPixels, 1, GenerationParams.RangeBounds);
        }

        private ColorTransformation CreateRandomColorTransformation()
        {
            return ColorTransformation.CreateRandomPolynomialColorTransformation(_random,
                GenerationParams.ColorChannelPolinomialTransformationCoefficientBounds,
                GenerationParams.ColorChannelZeroProbabilty);
        }

        private FormulaTree CreateRandomFormulaTree()
        {
            int dimensionsCount = _random.Next(GenerationParams.DimensionCountBounds);
            int minimalDepth = _random.Next(GenerationParams.MinimalDepthBounds);
            double constantProbability = _random.Next(GenerationParams.ConstantProbabilityBounds);
            double leafProbability = _random.Next(GenerationParams.LeafProbabilityBounds);
            IDictionary<Operator, double> operatorAndProbabilityMap = GenerationParams.OperatorAndProbabilityMap.ToDictionary(e => e.Key, e => e.Value);
            Func<double> createConst = () =>
            {
                double c = Math.Round(_random.Next(GenerationParams.ConstantBounds), 2);
                return Math.Abs(c - 0) < 0.01 ? 0.01 : c;
            };

            return FormulaTreeGenerator.Generate(_random, operatorAndProbabilityMap, createConst, dimensionsCount, minimalDepth, 
                leafProbability, constantProbability);
        }

        public async Task<FormulaRenderResult> RenderFormulaAsync(ProgressObserver progressObserver)
        {
            if (FormulaRenderArguments == null)
                throw new InvalidOperationException("FormulaRenderArguments is null.");

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

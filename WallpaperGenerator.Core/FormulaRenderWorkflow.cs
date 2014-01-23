using System;
using System.Diagnostics;
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
        private Size _imageSize;
        private FormulaRenderArguments _formulaRenderArguments;
        private double[] _lastEvaluatedFormulaValues;

        public FormulaRenderArgumentsGenerationParams GenerationParams { get; set; }

        public Size ImageSize
        {
            get { return _imageSize; }
            set
            {
                _imageSize = value;
                if (_formulaRenderArguments != null)
                {
                    _formulaRenderArguments.ImageSize = value;
                }
            }
        }

        public FormulaRenderArguments FormulaRenderArguments
        {
            get { return _formulaRenderArguments; }
            set 
            {
                if (_formulaRenderArguments == null || _formulaRenderArguments.ToString() != value.ToString() || _formulaRenderArguments.ImageSize != value.ImageSize)
                {
                    _formulaRenderArguments = value;
                    _lastEvaluatedFormulaValues = null;
                }
            }
        }

        public FormulaRenderResult LastFormulaRenderResult { get; private set; }

        public bool IsImageReady
        {
            get { return _lastEvaluatedFormulaValues != null && !IsImageRendering; }
        }

        public bool IsImageRendering { get; private set; }

        public FormulaRenderWorkflow(FormulaRenderArgumentsGenerationParams generationParams, Size imageSize)
            : this(generationParams, imageSize, new Random())
        {
        }

        public FormulaRenderWorkflow(FormulaRenderArgumentsGenerationParams generationParams, Size imageSize, Random random)
        {
            GenerationParams = generationParams;
            ImageSize = imageSize;
            _random = random;
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
            return RangesForFormula2DProjection.CreateRandom(_random, variablesCount, ImageSize, 1, GenerationParams.RangeBounds);
        }

        private ColorTransformation CreateRandomColorTransformation()
        {
            return ColorTransformation.CreateRandomPolynomialColorTransformation(_random,
                GenerationParams.ColorChannelPolinomialTransformationCoefficientBounds,
                GenerationParams.ColorChannelZeroProbabilty);
        }

        private FormulaTree CreateRandomFormulaTree()
        {
            FormulaGenerationArguments args = FormulaGenerationArguments.CreateRandom(_random, GenerationParams.DimensionCountBounds,
                GenerationParams.MinimalDepthBounds, GenerationParams.LeafProbabilityBounds, GenerationParams.ConstantProbabilityBounds,
                GenerationParams.ConstantBounds, GenerationParams.OperatorAndMaxProbabilityBoundsMap,
                GenerationParams.ObligatoryOperators, GenerationParams.UnaryVsBinaryOperatorsProbabilityBounds);

            return FormulaTreeGenerator.Generate(_random, args.OperatorAndProbabilityMap, args.CreateConstant,
                args.DimensionsCount, args.MinimalDepth, args.LeafProbability, args.ConstantProbability);
        }

        public async Task<FormulaRenderResult> RenderFormulaAsync(ProgressObserver progressObserver)
        {
            if (FormulaRenderArguments == null)
                throw new InvalidOperationException("FormulaRenderArguments is null.");

            IsImageRendering = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double evaluationProgressSpan = 0;
            if (_lastEvaluatedFormulaValues == null)
            {
                evaluationProgressSpan = 0.97;
                _lastEvaluatedFormulaValues = await EvaluateFormulaAsync(FormulaRenderArguments, evaluationProgressSpan, progressObserver);
            }

            RenderedFormulaImage renderedFormulaImage = await RenderFormulaAsync(_lastEvaluatedFormulaValues, FormulaRenderArguments.ImageSize,
                FormulaRenderArguments.ColorTransformation,
                1 - evaluationProgressSpan, evaluationProgressSpan, progressObserver);

            stopwatch.Stop();
            IsImageRendering = false;
            return LastFormulaRenderResult = new FormulaRenderResult(FormulaRenderArguments, renderedFormulaImage, stopwatch.Elapsed);
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

        private static Task<RenderedFormulaImage> RenderFormulaAsync(double[] evaluatedFormulaValues, Size imageSize, ColorTransformation colorTransformation,
            double progressSpan, double initProgress, ProgressObserver progressObserver)
        {
            return Task.Run(() =>
            {
                if (progressObserver != null)
                    ProgressReporter.Subscribe(progressObserver);
                using (ProgressReporter.CreateScope(progressSpan, initProgress))
                    return FormulaRender.Render(evaluatedFormulaValues, imageSize, colorTransformation);
            });
        }
    }
}

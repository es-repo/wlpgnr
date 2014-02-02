using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.App.Core
{
    public class FormulaRenderWorkflow
    {
        private readonly Random _random = new Random();
        private readonly int _numberOfCores;
        private readonly FormulaGoodnessAnalyzer _formulaGoodnessAnalyzer;
        private Size _imageSize;
        private FormulaRenderArguments _formulaRenderArguments;
        private FormulaRenderResult _formulaRenderResult;
        private FormulaBitmap _bitmap;
        private readonly Func<Size, FormulaBitmap> _createFormulaBitmap; 
        private bool _reevaluateValues;
        private int _generatedFormulasCount;

        public FormulaRenderArgumentsGenerationParams GenerationParams { get; set; }

        public Size ImageSize
        {
            get { return _imageSize; }
            set
            {
                if (_imageSize == value)
                    return;

                _imageSize = value;
                _formulaRenderResult = new FormulaRenderResult(value);
                _bitmap = _createFormulaBitmap(_imageSize);
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
                    _reevaluateValues = true;
                }
            }
        }

        public WorkflowRenderResult LastWorkflowRenderResult { get; private set; }

        public bool IsImageReady
        {
            get { return !_reevaluateValues && !IsImageRendering; }
        }

        public bool IsImageRendering { get; private set; }

        public FormulaRenderWorkflow(FormulaRenderArgumentsGenerationParams generationParams, Size imageSize, Func<Size, FormulaBitmap> createFormulaBitmap, int numberOfCores)
            : this(generationParams, imageSize, new FormulaGoodnessAnalyzer(generationParams.DimensionCountBounds.Low), createFormulaBitmap, numberOfCores, new Random())
        {
        }

        public FormulaRenderWorkflow(FormulaRenderArgumentsGenerationParams generationParams, Size imageSize, FormulaGoodnessAnalyzer formulaGoodnessAnalyzer,
            Func<Size, FormulaBitmap> createFormulaBitmap, int numberOfCores, Random random)
        {
            GenerationParams = generationParams;
            _formulaGoodnessAnalyzer = formulaGoodnessAnalyzer;
            _createFormulaBitmap = createFormulaBitmap;
            _random = random;
            _numberOfCores = numberOfCores;
            _reevaluateValues = true;
            ImageSize = imageSize;
        }

        public Task<FormulaRenderArguments> GenerateFormulaRenderArgumentsAsync(double progressSpan, double initProgress, ProgressObserver progressObserver)
        {
            return Task.Run(() =>
            {
                if (progressObserver != null)
                    ProgressReporter.Subscribe(progressObserver);
                using (ProgressReporter.CreateScope(progressSpan, initProgress))
                    return GenerateFormulaRenderArguments();
            });
        }

        public FormulaRenderArguments GenerateFormulaRenderArguments()
        {
            FormulaRenderArguments args = GenerationParams.PredefinedFormulaRenderingArgumentsEnabled ? GetPredefinedFormulaRenderArguments() : null;
            
            if (args == null)
            {
                const int attemptCount = 10;
                using (ProgressReporter.CreateScope(attemptCount))
                {
                    FormulaTree formulaTree = FuncUtilities.Repeat(() =>
                    {
                        FormulaTree f = CreateRandomFormulaTree();
                        ProgressReporter.Increase();
                        return f;
                    },
                    f => _formulaGoodnessAnalyzer.Analyze(f), attemptCount);

                    RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length);
                    ColorTransformation colorTransformation = CreateRandomColorTransformation();
                    args = new FormulaRenderArguments(formulaTree, ranges, colorTransformation);
                }
            }

            _generatedFormulasCount++;
            return FormulaRenderArguments = args;
        }

        private FormulaRenderArguments GetPredefinedFormulaRenderArguments()
        {
            if (_generatedFormulasCount < GenerationParams.FirstPredefinedFormulaRenderingArgumentsCount)
            {
                string s = GenerationParams.PredefinedFormulaRenderingFormulaRenderArgumentStrings.TakeRandom(_random);
                FormulaRenderArguments args = FormulaRenderArguments.FromString(s);
                return TransformRanges(args);
            }

            if ((_generatedFormulasCount - GenerationParams.FirstPredefinedFormulaRenderingArgumentsCount + 1) %
                     GenerationParams.RepeatPredefinedFormulaRenderingArgumentsAfterEvery == 0)
            {
                string s = GenerationParams.PredefinedFormulaRenderingFormulaRenderArgumentStrings.TakeRandom(_random);
                FormulaRenderArguments args = FormulaRenderArguments.FromString(s);
                args = TransformRanges(args);
                return ChangeColors(args);
            }

            return null;
        }

        public FormulaRenderArguments ChangeColors()
        {
            return _formulaRenderArguments = ChangeColors(FormulaRenderArguments);
        }

        private FormulaRenderArguments ChangeColors(FormulaRenderArguments args)
        {
            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            return new FormulaRenderArguments(args.FormulaTree, args.Ranges, colorTransformation);
        }

        public FormulaRenderArguments TransformRanges()
        {
            return FormulaRenderArguments = TransformRanges(FormulaRenderArguments);
        }

        private FormulaRenderArguments TransformRanges(FormulaRenderArguments args)
        {
            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(args.FormulaTree.Variables.Length);
            return new FormulaRenderArguments(
                args.FormulaTree,
                ranges,
                args.ColorTransformation);
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

        public Task<WorkflowRenderResult> BenchmarkAsync(ProgressObserver progressObserver)
        {
            FormulaRenderArguments = FormulaRenderArguments.FromString(
                @"-8.52,0.9;0.65,10.42;-7.37,-0.31;-0.89,2.79;0.31,1.9;0.52,6.01;2.29,8.68
-1.93,-2.17,-0.59,0.21;-0.94,-2.78,0.06,0.12;3.9,3.25,-2.59,0.04
Sub Sqrt Sqrt Cos Sub Sub Sub Sum Cos Atan Ln Cos Sub x5 x0 Cos Atan Pow3 Cbrt Sum x5 x5 Sin Cbrt Cbrt Cos Sqrt Mul x5 x0 Cbrt Sub Mul Sub Mul Mul Sum x3 x5 Sum x4 x6 Sum Sub x1 x0 Sin x3 Sub Cbrt Sub x2 x3 Sum Sum x3 x2 Sub x1 x0 Cbrt Mul Sub Cbrt x6 Sub x2 x2 Sub Cbrt x6 Mul x3 x1 Sub Cbrt Sub Sub Sub x3 x2 Sub x4 x0 Atan Pow3 x6 Max Cos Cbrt Mul x6 x1 Sum Cbrt Mul x4 x3 Atan Ln x3 Sub Sub Sin Sum Mul Sum Atan Ln x5 Mul Sum x1 x0 Sin x4 Cbrt Sub Cos x1 Sum x3 x3 Mul Sin Max Mul x2 x0 Sin x0 Sub Sqrt Sum x2 x5 Cbrt Sub x2 x1 Sub Cbrt Sub Sin Sub Cbrt x4 Sub x6 x3 Sub Cbrt Sub x2 x0 Sub Cos x0 Sum x0 x2 Cos Sum Mul Sqrt Sub x6 x0 Sqrt Cos x5 Sub Sqrt Cos x3 Max Cos x1 Mul x2 x2 Sum Sum Cbrt Sub Cos Cbrt Sub x1 x5 Sub Atan Atan Ln x2 Sub Sub x6 x2 Sum x6 x4 Max Sub Sum Sub Sub x1 x6 Sub x5 x2 Cbrt Sub x3 x1 Sum Sum Cos x0 Sub x2 x2 Cos Cos x4 Sin Sqrt Cos Cos x6 Atan Sub Sqrt Sin Sub Cos x1 Sum x6 x0 Sub Sub Cbrt Cos x2 Sum Sub x1 x4 Cos x2 Sum Mul Sum x2 x1 Sum x2 x0 Sub Sum x1 x4 Sum x2 x5 Sub Cbrt Sub Sum Sin Cos Sub Sub Sqrt Max Max Sub x3 x4 Sum x0 x0 Sum Atan Ln x5 Atan Pow3 x4 Mul Sub Sqrt Atan Ln x5 Sub Cbrt x2 Sub x3 x4 Sub Sub Sub x3 x6 Sub x1 x6 Sub Sub x1 x4 Sin x3 Max Cbrt Sqrt Cos Sub x3 x5 Sum Sub Cbrt Sub x4 x1 Sum Cos x6 Sin x5 Sub Atan Atan Pow3 x4 Cbrt Sum x5 x3 Atan Ln Sum Sin Cbrt Cos Cos Sum x4 x3 Cbrt Atan Pow3 Sqrt Sub x4 x2 Cbrt Sub Sum Sub 8.3 Atan Pow3 Sqrt Sub Sum x3 x2 Cos x3 Sum Cos Cbrt Sin Atan Cos x4 Cbrt Sum Cos Sqrt Cos x6 Sub Sub Sum x5 x2 Sum x4 x5 Sum Sub x5 x4 Cbrt x6 Sub Cbrt Cos Sub Sub Cos Sub x6 x0 Sin Cos x0 Sum Mul Sum x0 x4 Cos x6 Sqrt Mul x6 x3 Sub Cbrt Sum Sum Max Sub x6 x6 Cos x1 Sin Mul x6 x4 Sub Sum Sub x4 x5 Sqrt x6 Cos Sum x4 x5 Sin Cbrt Sum Atan Ln x1 Sqrt Sub x3 x4 Sub Cbrt Sqrt Sum Mul Sum Sum Atan Ln Cbrt Cos x3 Max Sum Sub Sum x6 x1 Mul x3 x4 Cbrt Sum x2 x6 Atan Pow3 Mul x4 x2 Mul Sub Sub Cos Sum x4 x2 Sub Cos x4 Sub x0 x3 Cbrt Sqrt Sum x0 x0 Sub Cos Sum Sum x3 x3 Atan Ln x6 Sub Cbrt Sub x1 x1 Sum Sum x3 x2 Cbrt x4 Cbrt Atan Sqrt Mul Cos Sub x3 x6 Sub Sub x1 x5 Sqrt x2 Atan Mul Sum Cos Cbrt Sub Sqrt x4 Atan Ln x4 Sub Mul Sqrt Sub x5 x4 Max Mul x2 x3 Sum x5 x4 Sum Sub Sub x6 x4 Cos x6 Sum Max x5 x6 Sub x3 x1 Cbrt Sub Mul Sub Sqrt x3 Sum x6 x4 Atan Ln x5 Sum Sum Sin x5 Sum x5 x1 Cos Sqrt x6 Max Sum Sin Sub Cos Atan Ln Cbrt Sub Sqrt x2 Cos x2 Sum Sum Sum Atan Ln Mul x1 x0 Sub Sum Atan Ln x3 Cbrt x6 Cbrt Cos x3 Sum Cos Cbrt Sum x6 x0 Sum Cos Sub x4 x1 Sub Sub x6 x5 Cbrt x5 Sqrt Atan Ln Sub Sub x4 x4 Max x5 x5 Sum Sqrt Cbrt Cos Sub Sub Atan Pow3 x4 Sin Cbrt x6 Sub Cos Cbrt x1 Cbrt Sub x6 x0 Sqrt Sub Sum Cbrt Max Sqrt Mul x4 x4 Cbrt Cbrt x1 Sum Sub Cos Sqrt x6 Cbrt Cbrt x6 Sub Mul Sqrt x6 Sub x3 x0 Sum Sub x2 x2 Sub x5 x0 Sqrt Cbrt Sum Sub Sum x5 x1 Max x1 x0 Sin Cos x4 Sum Sub Sum Sum Sin Sum Sub Atan Pow3 x4 Sum Mul x2 x5 Sqrt x1 Sin Sqrt Cos x5 Cbrt Mul Sub Cbrt Cos x4 Mul Sub x6 x1 Sqrt x3 Mul Atan Ln x2 Atan Sub x6 x6 Cos Sin Cos Sum Sub Sub x5 x3 Sum x6 x6 Cbrt Sum x0 x6 Cos Sub Sin Sub Sqrt Sub Sub x4 x3 Sqrt x6 Sub Cos Atan Pow3 x5 Sqrt Cos x4 Sub Sqrt Sub Atan Pow3 x1 Cbrt Sub x4 x6 Sub Sub Sub Sub x0 x6 Atan Ln x0 Sqrt Sqrt x1 Sum Sub Sqrt x5 Sub x6 x1 Cos Atan Ln x4 Cbrt Cbrt Sqrt Sum Sin Sub Sub Atan Pow3 x6 Atan Pow3 x4 Sum Sum x4 x4 Mul x4 x3 Sub Sin Mul Sub x2 x6 Atan Ln x4 Sum Sub Sin x1 Atan Pow3 x6 Sub Atan Ln x4 Sub x2 x6");
            
            FormulaRenderArguments.ImageSize = ImageSize;

            return RenderFormulaAsync(false, progressObserver);
        }

        public Task<WorkflowRenderResult> RenderFormulaAsync(bool generateNew, ProgressObserver progressObserver)
        {
            if (FormulaRenderArguments == null && !generateNew)
                throw new InvalidOperationException("FormulaRenderArguments is null.");

            return Task.Run(() =>
            {
                if (progressObserver != null)
                    ProgressReporter.Subscribe(progressObserver);
                return RenderFormula(generateNew);
            });
        }

        public WorkflowRenderResult RenderFormula(bool generateNew)
        {
            if (FormulaRenderArguments == null && !generateNew)
                throw new InvalidOperationException("FormulaRenderArguments is null.");

            using (ProgressReporter.CreateScope())
            {
                IsImageRendering = true;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
            
                double formulaGenerationrProgressSpan = 0;
                if (generateNew)
                {
                    formulaGenerationrProgressSpan = 0.01;
                    using (ProgressReporter.CreateScope(formulaGenerationrProgressSpan))
                        FormulaRenderArguments = GenerateFormulaRenderArguments();
                }

                using (ProgressReporter.CreateScope(1 - formulaGenerationrProgressSpan))
                {
                    FormulaRender.Render(FormulaRenderArguments.FormulaTree, FormulaRenderArguments.Ranges, FormulaRenderArguments.ColorTransformation, _reevaluateValues, _numberOfCores, _formulaRenderResult);
                    _reevaluateValues = false;
                }

                stopwatch.Stop();
                IsImageRendering = false;
                return LastWorkflowRenderResult = new WorkflowRenderResult(FormulaRenderArguments, _formulaRenderResult, _bitmap, stopwatch.Elapsed);
            }
        }
    }
}

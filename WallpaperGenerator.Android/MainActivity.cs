using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Android.OS;
using WallpaperGenerator.Core;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.Orientation)]
    public class MainActivity : Activity
    {
        private readonly Random _random = new Random();
        private TextView _formulaTextView;
        private ImageView _wallpaperImageView;
        private FormulaRenderingArguments _formulaRenderingArguments;
        private double[] _lastEvaluatedFormulaValues;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            
            _formulaTextView = FindViewById<TextView>(Resource.Id.formulaTextView);
            _wallpaperImageView = FindViewById<ImageView>(Resource.Id.wallpaperImageView);

            ClearWallpaperImageView();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.generateMenuItem:
                    OnGenerateMenuItemSelected();
                    return true;

                case Resource.Id.renderMenuItem:
                    OnRenderMenuItemSelected();
                    return true;

                case Resource.Id.changeColorMenuItem:
                    OnChangeColorMenuItemSelected();
                    return true;

                case Resource.Id.transformMenuItem:
                    OnTransformMenuItemSelected();
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private async void OnGenerateMenuItemSelected()
        {
            _formulaRenderingArguments = await GenerateRandomFormulaRenderingArgumentsAsync();
            _formulaTextView.Text = _formulaRenderingArguments.ToString();
        }

        private async void OnRenderMenuItemSelected()
        {
            if (_formulaRenderingArguments == null)
                return;

            ClearWallpaperImageView();
            await RenderWallpaperBitmapAsync(_formulaRenderingArguments, true);
        }

        private async void OnChangeColorMenuItemSelected()
        {
            if (_formulaRenderingArguments == null)
                return;

            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            _formulaRenderingArguments = new FormulaRenderingArguments(
                _formulaRenderingArguments.FormulaTree,
                _formulaRenderingArguments.Ranges,
                colorTransformation);
            _formulaTextView.Text = _formulaRenderingArguments.ToString();
            await RenderWallpaperBitmapAsync(_formulaRenderingArguments, false);
        }

        private async void OnTransformMenuItemSelected()
        {
            if (_formulaRenderingArguments == null)
                return;

            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(_formulaRenderingArguments.FormulaTree.Variables.Length);
            _formulaRenderingArguments = new FormulaRenderingArguments(
                _formulaRenderingArguments.FormulaTree,
                ranges,
                _formulaRenderingArguments.ColorTransformation);
            _formulaTextView.Text = _formulaRenderingArguments.ToString();
            await RenderWallpaperBitmapAsync(_formulaRenderingArguments, true);
        }

        private void ClearWallpaperImageView()
        {
            if (_formulaRenderingArguments == null)
                return;
            int width = _formulaRenderingArguments.Ranges.XCount; 
            int height = _formulaRenderingArguments.Ranges.YCount; 
            int[] pixels = new int[width * height];
            Bitmap blankBitmap = Bitmap.CreateBitmap(pixels, width, height, Bitmap.Config.Argb8888);
            _wallpaperImageView.SetImageBitmap(blankBitmap);
        }

        private async Task RenderWallpaperBitmapAsync(FormulaRenderingArguments args, bool reevaluateFormula)
        {
            Tuple<RenderedFormulaImage, double[]> formulaImageAndValues = await RenderFormulaAsync(args, reevaluateFormula ? null : _lastEvaluatedFormulaValues);
            _lastEvaluatedFormulaValues = formulaImageAndValues.Item2;
            RenderedFormulaImage formulaImage = formulaImageAndValues.Item1;
            
            int length = formulaImage.RedChannel.Length;
            int[] pixels = new int[length];
            for (int i = 0; i < length; i++)
                pixels[i] = Color.Argb(255, formulaImage.RedChannel[i], formulaImage.GreenChannel[i], formulaImage.BlueChannel[i]);

            Bitmap bitmap = Bitmap.CreateBitmap(pixels, formulaImage.WidthInPixels, formulaImage.HeightInPixels, Bitmap.Config.Argb8888);
            _wallpaperImageView.SetImageBitmap(bitmap);
        }

        private Task<FormulaRenderingArguments> GenerateRandomFormulaRenderingArgumentsAsync()
        {
            return Task.Run(() => GenerateRandomFormulaRenderingArguments());
        }

        // TODO: Move to Core.
        private FormulaRenderingArguments GenerateRandomFormulaRenderingArguments()
        {
            FormulaTree formulaTree = CreateRandomFormulaTree();
            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length);
            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            return new FormulaRenderingArguments(formulaTree, ranges, colorTransformation);
        }

        // TODO: Move to Core.
        private RangesForFormula2DProjection CreateRandomVariableValuesRangesFor2DProjection(int variablesCount)
        {
            const int xRangeCount = Configuration.DefaultImageWidth; // TODO: Take screen pixels width.
            const int yRangeCount = Configuration.DefaultImageHeight; // TODO: Take screen pixels height.
            return RangesForFormula2DProjection.CreateRandom(_random, variablesCount, xRangeCount, yRangeCount, 1, Configuration.RangeLowBound, Configuration.RangeHighBound);
        }

        // TODO: Move to Core.
        private ColorTransformation CreateRandomColorTransformation()
        {
            return ColorTransformation.CreateRandomPolynomialColorTransformation(_random,
                Configuration.ColorChannelPolinomialTransformationCoefficientLowBound,
                Configuration.ColorChannelPolinomialTransformationCoefficientHighBound,
                Configuration.ColorChannelZeroProbabilty);
        }

        // TODO: Move to Core.
        private FormulaTree CreateRandomFormulaTree()
        {
            int dimensionsCount = _random.Next(Configuration.DimensionCountLowBound, Configuration.DimensionCountHighBound);
            int minimalDepth = _random.Next(Configuration.MinimalDepthLowBound, Configuration.MinimalDepthHighBound);
            double constantProbability = _random.Next(Configuration.ConstantProbabilityLowBound, Configuration.ConstantProbabilityHighBound);
            double varOrConstantProbability = _random.Next(Configuration.VariableOrConstasntProbabilityLowBound, Configuration.VariableOrConstasntProbabilityLowBound);

            Operator[] operators = { OperatorsLibrary.Sum, OperatorsLibrary.Sub, OperatorsLibrary.Ln, OperatorsLibrary.Sin };
            IDictionary<Operator, double> operatorAndProbabilityMap = operators.ToDictionary(op => op, op => 0.5);
            
            Func<double> createConst = () =>
            {
                double c = _random.Next(Configuration.ConstantLowBound, Configuration.ConstantHighBound);
                return Math.Abs(c - 0) < 0.01 ? 0.01 : c;
            };

            return FormulaTreeGenerator.Generate(operatorAndProbabilityMap, createConst, dimensionsCount, minimalDepth, _random, varOrConstantProbability, constantProbability);
        }

        // TODO: move to Core.
        private static async Task<Tuple<RenderedFormulaImage, double[]>> RenderFormulaAsync(FormulaRenderingArguments formulaRenderingArguments, double[] evaluatedFormulaValues)
        {
            //double evaluationProgressSpan = 0;
            if (evaluatedFormulaValues == null)
            {
                //evaluationProgressSpan = 0.98;
                evaluatedFormulaValues = await EvaluateFormulaAsync(formulaRenderingArguments/*, evaluationProgressSpan/*, renderingProgressObserver*/);
            }

            RenderedFormulaImage renderedFormulaImage = await RenderFormulaAsync(evaluatedFormulaValues, formulaRenderingArguments.WidthInPixels,
                formulaRenderingArguments.HeightInPixels, formulaRenderingArguments.ColorTransformation
                /*1 - evaluationProgressSpan, evaluationProgressSpan, renderingProgressObserver*/);

            return Tuple.Create(renderedFormulaImage, evaluatedFormulaValues);
        }

        // TODO: move to Core.
        private static Task<double[]> EvaluateFormulaAsync(FormulaRenderingArguments formulaRenderingArguments/*, double progressSpan, ProgressObserver progressObserver*/)
        {
            return Task.Run(() =>
            {
                //ProgressReporter.Subscribe(progressObserver);
                //using (ProgressReporter.CreateScope(progressSpan))
                    return FormulaRender.EvaluateFormula(formulaRenderingArguments.FormulaTree, formulaRenderingArguments.Ranges);
            });
        }

        // TODO: move to Core.
        private static Task<RenderedFormulaImage> RenderFormulaAsync(double[] evaluatedFormulaValues, int widthInPixels, int heightInPixels, ColorTransformation colorTransformation
            /*double progressSpan, double initProgress, ProgressObserver progressObserver*/)
        {
            return Task.Run(() =>
            {
                //ProgressReporter.Subscribe(progressObserver);
                //using (ProgressReporter.CreateScope(progressSpan, initProgress))
                    return FormulaRender.Render(evaluatedFormulaValues, widthInPixels, heightInPixels, colorTransformation);
            });
        }
    }
}


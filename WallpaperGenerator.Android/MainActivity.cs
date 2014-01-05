using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Utilities;
using Android.Views;
using Android.Widget;
using Android.OS;
using WallpaperGenerator.Core;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.Orientation)]
    public class MainActivity : BaseActivity
    {
        private readonly Random _random = new Random();
        private TextView _formulaTextView;
        private TextView _progressTextView;
        private TextView _renderTimeTextView;
        private ImageView _wallpaperImageView;
        private FormulaRenderingArguments _formulaRenderingArguments;
        private double[] _lastEvaluatedFormulaValues;

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);

                SetContentView(Resource.Layout.Main);

                _formulaTextView = FindViewById<TextView>(Resource.Id.formulaTextView);
                _progressTextView = FindViewById<TextView>(Resource.Id.progressTextView);
                _renderTimeTextView = FindViewById<TextView>(Resource.Id.renderTimeTextView);
                _wallpaperImageView = FindViewById<ImageView>(Resource.Id.wallpaperImageView);

                _formulaRenderingArguments = FormulaRenderingArguments.FromString(
                    @"720;1280;1;1.45;4.31,0.0299;-2.79,0.0193;-0.72,0.0322;-16.12,0.0275;-15.81,0.0524;-11.44,0.0124;-5.41,0.0137;-4.97,0.0156
1.93,13.3,6.87,0.16;0.13,0,1.72,0.04;2.23,-3.87,-2.57,0.24
Sum Sub Atan Ln Sin Sub Tanh Ln Sub Sin Atan Ln Sin x3 Sum Sin Sum x2 Sum x0 x6 Tanh Ln Sum x6 x4 Sin Tanh Ln Tanh Ln Sin Sin x7 Atan Ln Sum Atan Ln Sub Tanh Ln Atan Ln Sin x7 Sin Sum 4.63 Atan Ln Sin x7 Sin Sub Tanh Ln Sum Sub Atan Ln x1 Tanh Ln x1 Sin Sum Sum x5 x3 Tanh Ln x6 Sub Sin Sum Sum Tanh Ln x6 x6 Sin Atan Ln x0 Sum Sin Sum Sub Sum x2 x1 Tanh Ln x1 Atan Ln x7 Sub Sum Sub x2 Sub x7 x1 Sin Sub x5 x4 Sub Tanh Ln x6 Sin Sum x2 x3 Sin Sin Sum Atan Ln Sum Sub Atan Ln Atan Ln Tanh Ln x1 Sum Sub Sum x5 Sub Sin x5 Sin x0 Sum Atan Ln x2 Sum Sub x7 x6 Sin x4 Atan Ln Sum x3 Sub x5 x1 Atan Ln Sin Sum Sin Sum x7 x3 Sub Atan Ln x4 Sub x3 x2 x5");
                _formulaTextView.Text = _formulaRenderingArguments.ToString();

                ClearWallpaperImageView();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, e.Message, ToastLength.Long).Show();
            }
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
            ProgressObserver renderingProgressObserver = new ProgressObserver(
                p => RunOnUiThread(() => _progressTextView.Text = ProgressToString(p.ProgressInPercents1d)));

            RenderFormulaResult renderFormulaResult = await RenderFormulaAsync(args, reevaluateFormula ? null : _lastEvaluatedFormulaValues, renderingProgressObserver);
            _lastEvaluatedFormulaValues = renderFormulaResult.FormulaEvaluatedValues;

            _renderTimeTextView.Text = renderFormulaResult.ElapsedTime.ToString();

            RenderedFormulaImage image = renderFormulaResult.Image;
            int length = image.RedChannel.Length;
            int[] pixels = new int[length];
            for (int i = 0; i < length; i++)
                pixels[i] = Color.Argb(255, image.RedChannel[i], image.GreenChannel[i], image.BlueChannel[i]);

            Bitmap bitmap = Bitmap.CreateBitmap(pixels, image.WidthInPixels, image.HeightInPixels, Bitmap.Config.Argb8888);
            _wallpaperImageView.SetImageBitmap(bitmap);
        }

        private static string ProgressToString(double value)
        {
            return Math.Round(value, 2).ToInvariantString() + "%";
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
        private static async Task<RenderFormulaResult> RenderFormulaAsync(FormulaRenderingArguments formulaRenderingArguments, double[] evaluatedFormulaValues,
            ProgressObserver progressObserver)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double evaluationProgressSpan = 0;
            if (evaluatedFormulaValues == null)
            {
                evaluationProgressSpan = 0.98;
                evaluatedFormulaValues = await EvaluateFormulaAsync(formulaRenderingArguments, evaluationProgressSpan, progressObserver);
            }

            RenderedFormulaImage renderedFormulaImage = await RenderFormulaAsync(evaluatedFormulaValues, formulaRenderingArguments.WidthInPixels,
                formulaRenderingArguments.HeightInPixels, formulaRenderingArguments.ColorTransformation,
                1 - evaluationProgressSpan, evaluationProgressSpan, progressObserver);

            stopwatch.Stop();
            return new RenderFormulaResult(renderedFormulaImage, evaluatedFormulaValues, stopwatch.Elapsed);
        }

        // TODO: move to Core.
        private static Task<double[]> EvaluateFormulaAsync(FormulaRenderingArguments formulaRenderingArguments, double progressSpan, ProgressObserver progressObserver)
        {
            return Task.Run(() =>
            {
                if (progressObserver != null)
                    ProgressReporter.Subscribe(progressObserver);
                using (ProgressReporter.CreateScope(progressSpan))
                    return FormulaRender.EvaluateFormula(formulaRenderingArguments.FormulaTree, formulaRenderingArguments.Ranges);
            });
        }

        // TODO: move to Core.
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


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Utilities;
using Android.Views;
using Android.Widget;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.UI.Android
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.Orientation)]
    public class MainActivity : BaseActivity
    {
        private TextView _formulaTextView;
        private TextView _progressTextView;
        private TextView _renderTimeTextView;
        private ImageView _imageView;

        private readonly Random _random = new Random();
        private int _imageWidth;
        private int _imageHeight;
        private FormulaRenderArguments _formulaRenderArguments;
        private double[] _lastEvaluatedFormulaValues;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _formulaTextView = FindViewById<TextView>(Resource.Id.formulaTextView);
            _progressTextView = FindViewById<TextView>(Resource.Id.progressTextView);
            _renderTimeTextView = FindViewById<TextView>(Resource.Id.renderTimeTextView);
            _imageView = FindViewById<ImageView>(Resource.Id.imageView);

            DisplayMetrics displayMetrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(displayMetrics);
            _imageWidth = displayMetrics.WidthPixels;
            _imageHeight = displayMetrics.HeightPixels;

            _formulaRenderArguments = FormulaRenderArguments.FromString(
@"720;1280;-5.35,6.11;-21.74,-0.21;2.98,20.68;-0.56,18.97;-2.6,1.76;-7.25,0.25;-5.88,3.87;-4.73,1.87;1;1.28
0,0,0,0;-0.93,0.5,-1.24,0.02;0.04,-0.92,0.47,0.05
Sum Sin Cbrt Sub Tanh Ln Cbrt Sin Sin Atan Ln Atan Ln x7 Atan Ln Sin Atan Ln Tanh Ln Sin Sum x3 x7 Atan Ln Sum Sum Cbrt Sub x1 Sum Cbrt Tanh Ln Sum Sin x0 Sub x2 x6 Sum Sin Sub x7 Sin Sub x7 x2 x3 Sub Sum Atan Ln Sum Tanh Ln Sum x6 x4 Sin Sin Tanh Ln x0 Tanh Ln Cbrt Cbrt Sum Sum x4 x5 Sum x3 x2 Sin Sin Atan Ln Sub Sub Sub x1 x1 Sum x4 x5 x6 Atan Ln Tanh Ln Atan Ln Sub -7.47 Sin Atan Ln x0"); 
            _formulaTextView.Text = _formulaRenderArguments.ToString();

            ClearImageView();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.Main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Task t;
            switch (item.ItemId)
            {
                case Resource.Id.generateMenuItem:
                    t = OnGenerateMenuItemSelected();
                    break;

                case Resource.Id.renderMenuItem:
                    t = OnRenderMenuItemSelected();
                    break;

                case Resource.Id.changeColorMenuItem:
                    t = OnChangeColorMenuItemSelected();
                    break;

                case Resource.Id.transformMenuItem:
                    t = OnTransformMenuItemSelected();
                    break;

                case Resource.Id.setAsWallpaperMenuItem:
                    OnSetAsWallpaperMenuItem();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }

            if (t == null)
                throw new InvalidOperationException();
            return true;
        }

        private async Task OnGenerateMenuItemSelected()
        {
            _formulaRenderArguments = await GenerateRandomFormulaRenderArgumentsAsync();
            _formulaTextView.Text = _formulaRenderArguments.ToString();
        }

        private async Task OnRenderMenuItemSelected()
        {
            if (_formulaRenderArguments == null)
                return;

            ClearImageView();
            await RenderImageAsync(_formulaRenderArguments, true);
        }

        private async Task OnChangeColorMenuItemSelected()
        {
            if (_formulaRenderArguments == null)
                return;

            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            _formulaRenderArguments = new FormulaRenderArguments(
                _formulaRenderArguments.FormulaTree,
                _formulaRenderArguments.Ranges,
                colorTransformation);
            _formulaTextView.Text = _formulaRenderArguments.ToString();
            await RenderImageAsync(_formulaRenderArguments, false);
        }

        private async Task OnTransformMenuItemSelected()
        {
            if (_formulaRenderArguments == null)
                return;

            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(_formulaRenderArguments.FormulaTree.Variables.Length);
            _formulaRenderArguments = new FormulaRenderArguments(
                _formulaRenderArguments.FormulaTree,
                ranges,
                _formulaRenderArguments.ColorTransformation);
            _formulaTextView.Text = _formulaRenderArguments.ToString();
            await RenderImageAsync(_formulaRenderArguments, true);
        }

        private void OnSetAsWallpaperMenuItem()
        {
            bool imgaeIsReady = _lastEvaluatedFormulaValues != null;
            if (!imgaeIsReady)
                return;

            Bitmap bitmap = ((BitmapDrawable)_imageView.Drawable).Bitmap;
            WallpaperManager wallpaperManager = WallpaperManager.GetInstance(this);
            
            // TODO: wrap in try..catch block
            wallpaperManager.SetBitmapWithExactScreenSize(bitmap);
            GoHome();
        }

        private void GoHome()
        {
            Intent startMain = new Intent(Intent.ActionMain);
            startMain.AddCategory(Intent.CategoryHome);
            StartActivity(startMain);
        }

        private void ClearImageView()
        {
            if (_formulaRenderArguments == null)
                return;
            int width = _formulaRenderArguments.Ranges.XCount; 
            int height = _formulaRenderArguments.Ranges.YCount; 
            int[] pixels = new int[width * height];
            Bitmap blankBitmap = Bitmap.CreateBitmap(pixels, width, height, Bitmap.Config.Argb8888);
            _imageView.SetImageBitmap(blankBitmap);
        }

        private async Task RenderImageAsync(FormulaRenderArguments args, bool reevaluateFormula)
        {
            ProgressObserver renderingProgressObserver = new ProgressObserver(
                p => RunOnUiThread(() => _progressTextView.Text = ProgressToString(p.ProgressInPercents1d)), TimeSpan.FromMilliseconds(100));

            FormulaRenderResult formulaRenderResult = await RenderFormulaAsync(args, reevaluateFormula ? null : _lastEvaluatedFormulaValues, renderingProgressObserver);
            _lastEvaluatedFormulaValues = formulaRenderResult.FormulaEvaluatedValues;

            _renderTimeTextView.Text = formulaRenderResult.ElapsedTime.ToString();

            RenderedFormulaImage image = formulaRenderResult.Image;
            int length = image.RedChannel.Length;
            int[] pixels = new int[length];
            for (int i = 0; i < length; i++)
                pixels[i] = Color.Argb(255, image.RedChannel[i], image.GreenChannel[i], image.BlueChannel[i]);

            Bitmap bitmap = Bitmap.CreateBitmap(pixels, image.WidthInPixels, image.HeightInPixels, Bitmap.Config.Argb8888);
            _imageView.SetImageBitmap(bitmap);
        }

        // TODO: Move to Core.
        private static string ProgressToString(double value)
        {
            return Math.Round(value, 2).ToInvariantString() + "%";
        }

        // TODO: Move to Core.
        private Task<FormulaRenderArguments> GenerateRandomFormulaRenderArgumentsAsync()
        {
            return Task.Run(() => GenerateRandomFormulaRenderArguments());
        }

        // TODO: Move to Core.
        private FormulaRenderArguments GenerateRandomFormulaRenderArguments()
        {
            FormulaTree formulaTree = CreateRandomFormulaTree();
            RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length);
            ColorTransformation colorTransformation = CreateRandomColorTransformation();
            return new FormulaRenderArguments(formulaTree, ranges, colorTransformation);
        }

        // TODO: Move to Core.
        private RangesForFormula2DProjection CreateRandomVariableValuesRangesFor2DProjection(int variablesCount)
        {
            return RangesForFormula2DProjection.CreateRandom(_random, variablesCount, _imageWidth, _imageHeight, 1, FormulaRenderConfiguration.RangeBounds);
        }

        // TODO: Move to Core.
        private ColorTransformation CreateRandomColorTransformation()
        {
            return ColorTransformation.CreateRandomPolynomialColorTransformation(_random,
                FormulaRenderConfiguration.ColorChannelPolinomialTransformationCoefficientBounds,
                FormulaRenderConfiguration.ColorChannelZeroProbabilty);
        }

        // TODO: Move to Core.
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

        // TODO: move to Core.
        private static async Task<FormulaRenderResult> RenderFormulaAsync(FormulaRenderArguments formulaRenderArguments, double[] evaluatedFormulaValues,
            ProgressObserver progressObserver)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double evaluationProgressSpan = 0;
            if (evaluatedFormulaValues == null)
            {
                evaluationProgressSpan = 0.97;
                evaluatedFormulaValues = await EvaluateFormulaAsync(formulaRenderArguments, evaluationProgressSpan, progressObserver);
            }

            RenderedFormulaImage renderedFormulaImage = await RenderFormulaAsync(evaluatedFormulaValues, formulaRenderArguments.WidthInPixels,
                formulaRenderArguments.HeightInPixels, formulaRenderArguments.ColorTransformation,
                1 - evaluationProgressSpan, evaluationProgressSpan, progressObserver);

            stopwatch.Stop();
            return new FormulaRenderResult(renderedFormulaImage, evaluatedFormulaValues, stopwatch.Elapsed);
        }

        // TODO: move to Core.
        private static Task<double[]> EvaluateFormulaAsync(FormulaRenderArguments formulaRenderArguments, double progressSpan, ProgressObserver progressObserver)
        {
            return Task.Run(() =>
            {
                if (progressObserver != null)
                    ProgressReporter.Subscribe(progressObserver);
                using (ProgressReporter.CreateScope(progressSpan))
                    return FormulaRender.EvaluateFormula(formulaRenderArguments.FormulaTree, formulaRenderArguments.Ranges);
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


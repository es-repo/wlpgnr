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
            Bitmap bitmap = await RenderWallpaperBitmapAsync(_formulaRenderingArguments);
            _wallpaperImageView.SetImageBitmap(bitmap);
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

        private async Task<Bitmap> RenderWallpaperBitmapAsync(FormulaRenderingArguments args)
        {
            RenderedFormulaImage formulaImage = await RenderFormulaImageAsync(args);
            int length = formulaImage.RedChannel.Length;
            int[] pixels = new int[length];
            for (int i = 0; i < length; i++)
                pixels[i] = Color.Argb(255, formulaImage.RedChannel[i], formulaImage.GreenChannel[i], formulaImage.BlueChannel[i]);

            return Bitmap.CreateBitmap(pixels, formulaImage.WidthInPixels, formulaImage.HeightInPixels, Bitmap.Config.Argb8888);
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
            const int yRangeCount = Configuration.DefaultImageWidth; // TODO: Take screen pixels height.
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

        private Task<RenderedFormulaImage> RenderFormulaImageAsync(FormulaRenderingArguments formulaRenderingArguments/*, 
            /*ProgressObserver renderingProgressObserver*/)
        {
            return Task.Run(() => FormulaRender.Render(
                formulaRenderingArguments.FormulaTree,
                formulaRenderingArguments.Ranges,
                formulaRenderingArguments.ColorTransformation));
        }
    }
}


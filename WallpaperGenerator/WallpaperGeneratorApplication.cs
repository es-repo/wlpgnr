using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.UI.Windows.MainWindowControls.ControlPanelControls;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.DataStructures.Collections;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.UI.Windows
{
    public class WallpaperGeneratorApplication : Application
    {        
        #region Fields

        private readonly Random _random = new Random();
        private readonly MainWindow _mainWindow;
        private WallpaperImage _wallpaperImage;
        private double[] _lastEvaluatedFormulaValues;
        private readonly FormulaRenderConfiguration _configuration;
        public const int imageWidth = 720;
        public const int imageHeight = 1280;
         
        #endregion

        #region Properties

        public FormulaRenderArguments GetCurrentFormulaRenderArguments()
        {
            string formuleString = _mainWindow.FormulaTexBox.Dispatcher.Invoke(() => _mainWindow.FormulaTexBox.Text);
            return formuleString != ""
                ? FormulaRenderArguments.FromString(formuleString)
                : null;
        }

        #endregion

        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _configuration = new FormulaRenderConfiguration();
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _mainWindow.ControlPanel.GenerateFormulaButton.Click += (s, a) =>
            {
                FormulaRenderArguments currentFormulaRenderArguments = GetCurrentFormulaRenderArguments();

                FormulaTree formulaTree = CreateRandomFormulaTree();
                
                RangesForFormula2DProjection ranges =
                    CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length, currentFormulaRenderArguments);

                ColorTransformation colorTransformation = CreateRandomColorTransformation();
                FormulaRenderArguments formulaRenderArguments = new FormulaRenderArguments(formulaTree, ranges, colorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString();
            };

            _mainWindow.ControlPanel.ChangeRangesButton.Click += async (s, a) =>
            {
                FormulaRenderArguments currentFormulaRenderArguments = GetCurrentFormulaRenderArguments();
                RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(
                        currentFormulaRenderArguments.FormulaTree.Variables.Length, currentFormulaRenderArguments);

                FormulaRenderArguments formulaRenderArguments = new FormulaRenderArguments(
                    currentFormulaRenderArguments.FormulaTree,
                    ranges,
                    currentFormulaRenderArguments.ColorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString();
                await RenderFormula(GetCurrentFormulaRenderArguments(), true);
            };

            _mainWindow.ControlPanel.ChangeColorButton.Click += async (s, a) =>
            {
                FormulaRenderArguments currentFormulaRenderArguments = GetCurrentFormulaRenderArguments(); 
                ColorTransformation colorTransformation = CreateRandomColorTransformation();
                FormulaRenderArguments formulaRenderArguments = new FormulaRenderArguments(
                    currentFormulaRenderArguments.FormulaTree, 
                    currentFormulaRenderArguments.Ranges, 
                    colorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString();
                await RenderFormula(GetCurrentFormulaRenderArguments(), false);
            };

            _mainWindow.ControlPanel.RenderFormulaButton.Click += async (s, a) => await RenderFormula(GetCurrentFormulaRenderArguments(), true);

            _mainWindow.ControlPanel.StartStopSmoothAnimationButton.Click += (s, a) => StartStopSmoothAnimation();

            _mainWindow.ControlPanel.SaveButton.Click += (s, a) => SaveFormulaImage();
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
        }

        private FormulaTree CreateRandomFormulaTree()
        {
            int dimensionsCount = (int)_mainWindow.ControlPanel.DimensionsCountSlider.Value;
            int minimalDepth = (int) _mainWindow.ControlPanel.MinimalDepthSlider.Value;
            double constantProbability = _mainWindow.ControlPanel.ConstantProbabilitySlider.Value/100;
            double leafProbability = _mainWindow.ControlPanel.LeafProbabilitySlider.Value / 100;
            
            IEnumerable<OperatorControl> checkedOperatorControls = _mainWindow.ControlPanel.OperatorControls.Where(cb => cb.IsChecked);
            IDictionary<Operator, double> operatorAndProbabilityMap =
                new DictionaryExt<Operator, double>(checkedOperatorControls.Select(opc => new KeyValuePair<Operator, double>(opc.Operator, opc.Probability)));

            Func<double> createConst = () => 
            {
                double d = _random.Next(_configuration.ConstantBounds);
                return Math.Abs(d - 0) < 0.01 ? 0.01 : d;
            };

            return FormulaTreeGenerator.Generate(operatorAndProbabilityMap, createConst, dimensionsCount, minimalDepth,
                _random, leafProbability, constantProbability);
        }

        private RangesForFormula2DProjection CreateRandomVariableValuesRangesFor2DProjection(int variablesCount,
            FormulaRenderArguments currentFormulaRenderArguments)
        {
            int xRangeCount = currentFormulaRenderArguments != null
                    ? currentFormulaRenderArguments.Ranges.XCount
                    : imageWidth;

            int yRangeCount = currentFormulaRenderArguments != null
                ? currentFormulaRenderArguments.Ranges.YCount
                : imageHeight;  
            
            return RangesForFormula2DProjection.CreateRandom(_random, variablesCount,
                xRangeCount, yRangeCount, 1, _configuration.RangeBounds);
        }

        private ColorTransformation CreateRandomColorTransformation()
        {
            return ColorTransformation.CreateRandomPolynomialColorTransformation(_random,
                _configuration.ColorChannelPolinomialTransformationCoefficientBounds,
                _configuration.ColorChannelZeroProbabilty);
        }

        private bool _isSmoothAnimationStarted;

        private void StartStopSmoothAnimation()
        {
            _isSmoothAnimationStarted = !_isSmoothAnimationStarted;
            if (_isSmoothAnimationStarted)
            {
                StartSmoothAnimation();
            }
        }

        private async void StartSmoothAnimation()
        {
            FormulaRenderArguments formulaRenderArguments = GetCurrentFormulaRenderArguments();
            Func<double[]> getNextRangeDeltas = () => EnumerableExtensions.Repeat(() => (-0.5 + _random.NextDouble()) * 0.1, formulaRenderArguments.Ranges.Ranges.Length).ToArray();
            
            double[] rangeStartDeltas = getNextRangeDeltas();
            double[] rangeEndDeltas = getNextRangeDeltas();

            Func<FormulaRenderArguments, FormulaRenderArguments> getNextFormulaRenderArguments = args =>
            {
                IEnumerable<Range> ranges = args.Ranges.Ranges.Select((r, i) => new Range(r.Start + rangeStartDeltas[i], r.End + rangeEndDeltas[i], r.Count));
                args.Ranges.Ranges = ranges.ToArray();
                return args;
            };
            
            int j = 0;
            while (_isSmoothAnimationStarted)
            {
                if (j > 200)
                {
                    j = 0;
                    rangeStartDeltas = getNextRangeDeltas();
                    rangeEndDeltas = getNextRangeDeltas();
                }

                j++;

                await DoAnimationStep(getNextFormulaRenderArguments);
            }
        }

        private async Task DoAnimationStep(Func<FormulaRenderArguments, FormulaRenderArguments> getNextFormulaRenderArguments)
        {
            FormulaRenderArguments formulaRenderArguments = GetCurrentFormulaRenderArguments();
            formulaRenderArguments = getNextFormulaRenderArguments(formulaRenderArguments);
            _mainWindow.FormulaTexBox.Dispatcher.Invoke(() => _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString());
            await RenderFormula(formulaRenderArguments, true);
        }

        private async Task RenderFormula(FormulaRenderArguments formulaRenderArguments, bool reevaluateFormula)
        {
            _mainWindow.Cursor = Cursors.Wait;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ProgressObserver renderingProgressObserver = new ProgressObserver(
                p => _mainWindow.StatusPanel.Dispatcher.Invoke(() => _mainWindow.StatusPanel.RenderingProgress = p.Progress));

            double evaluationProgressSpan = 0;
            if (reevaluateFormula)
            {
                evaluationProgressSpan = 0.98;
                _lastEvaluatedFormulaValues = await EvaluateFormulaAsync(formulaRenderArguments, evaluationProgressSpan, renderingProgressObserver);
            }

            RenderedFormulaImage renderedFormulaImage = await RenderFormulaAsync(_lastEvaluatedFormulaValues, formulaRenderArguments.WidthInPixels,
                formulaRenderArguments.HeightInPixels, formulaRenderArguments.ColorTransformation,
                1 - evaluationProgressSpan, evaluationProgressSpan, renderingProgressObserver);

            _wallpaperImage = new WallpaperImage(renderedFormulaImage.WidthInPixels, renderedFormulaImage.HeightInPixels);
            _wallpaperImage.Update(renderedFormulaImage);

            _mainWindow.WallpaperImage.Source = _wallpaperImage.Source;

            stopwatch.Stop();
            _mainWindow.StatusPanel.RenderedTime = stopwatch.Elapsed;
            _mainWindow.Cursor = Cursors.Arrow;
        }

        private Task<double[]> EvaluateFormulaAsync(FormulaRenderArguments formulaRenderArguments, double progressSpan, ProgressObserver progressObserver)
        {
            return Task.Run(() =>
            {
                ProgressReporter.Subscribe(progressObserver);
                using (ProgressReporter.CreateScope(progressSpan))
                    return FormulaRender.EvaluateFormula(formulaRenderArguments.FormulaTree, formulaRenderArguments.Ranges);
            });
        }

        private Task<RenderedFormulaImage> RenderFormulaAsync(double[] evaluatedFormulaValues, int widthInPixels, int heightInPixels, ColorTransformation colorTransformation,
            double progressSpan, double initProgress, ProgressObserver progressObserver)
        {
            return Task.Run(() =>
            {
                ProgressReporter.Subscribe(progressObserver);
                using (ProgressReporter.CreateScope(progressSpan, initProgress))
                    return FormulaRender.Render(evaluatedFormulaValues, widthInPixels, heightInPixels, colorTransformation);
            });
        }

        private void SaveFormulaImage()
        {
            if (_wallpaperImage != null)
                _wallpaperImage.SaveToFile("c:/temp/wlp.png");
        }
    }
}

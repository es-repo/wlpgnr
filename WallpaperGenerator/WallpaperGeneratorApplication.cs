using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.MainWindowControls.ControlPanelControls;
using WallpaperGenerator.Utilities.DataStructures.Collections;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator
{
    public class WallpaperGeneratorApplication : Application
    {        
        #region Fields

        private readonly Random _random = new Random();
        private readonly MainWindow _mainWindow;
        private WallpaperImage _wallpaperImage;
         
        #endregion

        #region Properties

        public FormulaRenderingArguments GetCurrentFormulaRenderingArguments()
        {
            string formuleString = _mainWindow.FormulaTexBox.Dispatcher.Invoke(() => _mainWindow.FormulaTexBox.Text);
            return formuleString != ""
                ? FormulaRenderingArguments.FromString(formuleString)
                : null;
        }

        #endregion

        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _mainWindow.ControlPanel.GenerateFormulaButton.Click += (s, a) =>
            {
                FormulaRenderingArguments currentFormulaRenderingArguments = GetCurrentFormulaRenderingArguments();

                FormulaTree formulaTree = CreateRandomFormulaTree();
                
                RangesForFormula2DProjection ranges =
                    CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length, currentFormulaRenderingArguments);

                ColorTransformation colorTransformation = CreateRandomColorTransformation();
                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(formulaTree, ranges, colorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
            };

            _mainWindow.ControlPanel.ChangeRangesButton.Click += async (s, a) =>
            {
                FormulaRenderingArguments currentFormulaRenderingArguments = GetCurrentFormulaRenderingArguments();
                RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(
                        currentFormulaRenderingArguments.FormulaTree.Variables.Length, currentFormulaRenderingArguments);

                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(
                    currentFormulaRenderingArguments.FormulaTree,
                    ranges,
                    currentFormulaRenderingArguments.ColorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
                await RenderFormula();
            };

            _mainWindow.ControlPanel.ChangeColorButton.Click += async (s, a) =>
            {
                FormulaRenderingArguments currentFormulaRenderingArguments = GetCurrentFormulaRenderingArguments(); 
                ColorTransformation colorTransformation = CreateRandomColorTransformation();
                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(
                    currentFormulaRenderingArguments.FormulaTree, 
                    currentFormulaRenderingArguments.Ranges, 
                    colorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
                await RenderFormula();
            };

            _mainWindow.ControlPanel.RenderFormulaButton.Click += async (s, a) => await RenderFormula();

            _mainWindow.ControlPanel.StartStopRandomAnimationButton.Click += (s, a) => StartStopAnimation();

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
            double varOrConstantProbability = _mainWindow.ControlPanel.VarOrConstantProbabilitySlider.Value / 100;
            
            IEnumerable<OperatorControl> checkedOperatorControls = _mainWindow.ControlPanel.OperatorControls.Where(cb => cb.IsChecked);
            IDictionary<Operator, double> operatorAndProbabilityMap =
                new DictionaryExt<Operator, double>(checkedOperatorControls.Select(opc => new KeyValuePair<Operator, double>(opc.Operator, opc.Probability)));

            Func<double> createConst = () => 
            {
                double d = Math.Round(_random.NextDouble() * (Configuration.FormulaConstHighBound - Configuration.FormulaConstLowBound) + Configuration.FormulaConstLowBound, 2);
                return Math.Abs(d - 0) < 0.01 ? 0.01 : d;
            };

            return FormulaTreeGenerator.Generate(operatorAndProbabilityMap, createConst, dimensionsCount, minimalDepth,
                _random, varOrConstantProbability, constantProbability);
        }

        private RangesForFormula2DProjection CreateRandomVariableValuesRangesFor2DProjection(int variablesCount, 
            FormulaRenderingArguments currentFormulaRenderingArguments)
        {
            int xRangeCount = currentFormulaRenderingArguments != null
                    ? currentFormulaRenderingArguments.Ranges.XCount
                    : Configuration.DefaultImageWidth;

            int yRangeCount = currentFormulaRenderingArguments != null
                ? currentFormulaRenderingArguments.Ranges.YCount
                : Configuration.DefaultImageWidth;  
            
            return RangesForFormula2DProjection.CreateRandom(_random, variablesCount,
                xRangeCount, yRangeCount,
                Configuration.RangeLowBound, Configuration.RangeHighBound);
        }

        private ColorTransformation CreateRandomColorTransformation()
        {
            return ColorTransformation.CreateRandomPolynomialColorTransformation(_random,
                Configuration.ColorChannelPolinomialTransformationCoefficientLowBound,
                Configuration.ColorChannelPolinomialTransformationCoefficientHighBound,
                Configuration.ColorChannelZeroProbabilty);
        }

        private bool _isAnimationStarted;

        private void StartStopAnimation()
        {
            _isAnimationStarted = !_isAnimationStarted;
            if (_isAnimationStarted)
            {
                StartAnimation();
            }
        }

        private async void StartAnimation()
        {
            while (_isAnimationStarted)
            {
                FormulaRenderingArguments currentFormulaRenderingArguments = GetCurrentFormulaRenderingArguments();
            
                RangesForFormula2DProjection ranges = CreateRandomVariableValuesRangesFor2DProjection(
                    currentFormulaRenderingArguments.FormulaTree.Variables.Length, currentFormulaRenderingArguments);

                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(
                    currentFormulaRenderingArguments.FormulaTree,
                    ranges,
                    currentFormulaRenderingArguments.ColorTransformation);

                _mainWindow.FormulaTexBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
                }));

                await RenderFormula();
            }
        }

        private async Task RenderFormula()
        {
            _mainWindow.Cursor = Cursors.Wait;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            FormulaRenderingArguments formulaRenderingArguments = GetCurrentFormulaRenderingArguments();
            
            ProgressObserver renderingProgressObserver = new ProgressObserver(
                p => _mainWindow.StatusPanel.Dispatcher.Invoke(() => _mainWindow.StatusPanel.RenderingProgress = p.ProgressInPercents1d));
            
            RenderedFormulaImage renderedFormulaImage = await GetRenderedFormulaImageAsync(formulaRenderingArguments, renderingProgressObserver);

            _wallpaperImage = new WallpaperImage(renderedFormulaImage.WidthInPixels, renderedFormulaImage.HeightInPixels);
            _wallpaperImage.Update(renderedFormulaImage);

            _mainWindow.WallpaperImage.Source = _wallpaperImage.Source;

            stopwatch.Stop();
            _mainWindow.StatusPanel.RenderedTime = stopwatch.Elapsed;
            _mainWindow.Cursor = Cursors.Arrow;
        }

        private async Task<RenderedFormulaImage> GetRenderedFormulaImageAsync(FormulaRenderingArguments formulaRenderingArguments, 
            ProgressObserver renderingProgressObserver)
        {
            RenderedFormulaImage renderedFormulaImage = null;
            await Task.Run(() =>
            {
                ProgressReporter.Subscribe(renderingProgressObserver);
                renderedFormulaImage = FormulaRender.Render(
                    formulaRenderingArguments.FormulaTree,
                    formulaRenderingArguments.Ranges,
                    formulaRenderingArguments.ColorTransformation);
            });
            return renderedFormulaImage;
        }

        private void SaveFormulaImage()
        {
            if (_wallpaperImage != null)
                _wallpaperImage.SaveToFile("c:/temp/wlp.png");
        }
    }
}

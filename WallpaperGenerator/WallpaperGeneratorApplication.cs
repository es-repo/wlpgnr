using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.UI.Core;
using WallpaperGenerator.UI.Windows.MainWindowControls.ControlPanelControls;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;

namespace WallpaperGenerator.UI.Windows
{
    public class WallpaperGeneratorApplication : Application
    {        
        #region Fields

        private readonly FormulaRenderWorkflow _workflow;
        private readonly MainWindow _mainWindow;
        private WallpaperImage _wallpaperImage;
        
        #endregion

        private FormulaRenderArguments UserInputFormulaRenderArguments
        {
            get
            {
                string formuleString = _mainWindow.FormulaTexBox.Dispatcher.Invoke(() => _mainWindow.FormulaTexBox.Text);
                return formuleString != ""
                    ? FormulaRenderArguments.FromString(formuleString)
                    : null;
            }
        }

        private FormulaRenderArgumentsGenerationParams UserInputFormulaRenderArgumentsGenerationParams
        {
            get
            {
                FormulaRenderArgumentsGenerationParams generationParams = _workflow.GenerationParams;
                int dimensionsCount = (int)_mainWindow.ControlPanel.DimensionsCountSlider.Value;
                generationParams.DimensionCountBounds = new Bounds<int>(dimensionsCount, dimensionsCount);

                int minimalDepth = (int) _mainWindow.ControlPanel.MinimalDepthSlider.Value;
                generationParams.MinimalDepthBounds = new Bounds<int>(minimalDepth, minimalDepth);

                double constantProbability = _mainWindow.ControlPanel.ConstantProbabilitySlider.Value/100;
                generationParams.ConstantProbabilityBounds = new Bounds(constantProbability, constantProbability);
                
                double leafProbability = _mainWindow.ControlPanel.LeafProbabilitySlider.Value / 100;
                generationParams.LeafProbabilityBounds = new Bounds(leafProbability, leafProbability);

                IEnumerable<OperatorControl> checkedOperatorControls = _mainWindow.ControlPanel.OperatorControls.Where(cb => cb.IsChecked);
                generationParams.Operators = checkedOperatorControls.Select(c => c.Operator).ToArray();
                //    IDictionary<Operator, double> operatorAndProbabilityMap =
                //        new DictionaryExt<Operator, double>(checkedOperatorControls.Select(opc => new KeyValuePair<Operator, double>(opc.Operator, opc.Probability)));

                FormulaRenderArguments renderArguments = UserInputFormulaRenderArguments;
                if (renderArguments != null)
                {
                    generationParams.WidthInPixels = renderArguments.WidthInPixels;
                    generationParams.HeightInPixels = renderArguments.HeightInPixels;
                }

                return generationParams;
            }
        }
        
        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _workflow = new FormulaRenderWorkflow(new FormulaRenderArgumentsGenerationParams());

            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _mainWindow.ControlPanel.GenerateFormulaButton.Click += (s, a) =>
            {
                _workflow.GenerationParams = UserInputFormulaRenderArgumentsGenerationParams;
                FormulaRenderArguments formulaRenderArguments = _workflow.GenerateFormulaRenderArguments();
                _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString();
            };

            _mainWindow.ControlPanel.TransformButton.Click += async (s, a) =>
            {
                _workflow.GenerationParams = UserInputFormulaRenderArgumentsGenerationParams;
                FormulaRenderArguments formulaRenderArguments = _workflow.TransformRanges();
                _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString();
                await DrawImageAsync();
            };

            _mainWindow.ControlPanel.ChangeColorButton.Click += async (s, a) =>
            {
                _workflow.GenerationParams = UserInputFormulaRenderArgumentsGenerationParams;
                FormulaRenderArguments formulaRenderArguments = _workflow.ChangeColors();
                _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString();
                await DrawImageAsync();
            };

            _mainWindow.ControlPanel.RenderFormulaButton.Click += async (s, a) =>
            {
                _workflow.FormulaRenderArguments = UserInputFormulaRenderArguments;
                await DrawImageAsync();
            };

            _mainWindow.ControlPanel.StartStopSmoothAnimationButton.Click += (s, a) => StartStopAnimation();

            _mainWindow.ControlPanel.SaveButton.Click += (s, a) => SaveFormulaImage();
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
        }

        //private FormulaTree CreateRandomFormulaTree()
        //{
        //    int dimensionsCount = (int)_mainWindow.ControlPanel.DimensionsCountSlider.Value;
        //    int minimalDepth = (int) _mainWindow.ControlPanel.MinimalDepthSlider.Value;
        //    double constantProbability = _mainWindow.ControlPanel.ConstantProbabilitySlider.Value/100;
        //    double leafProbability = _mainWindow.ControlPanel.LeafProbabilitySlider.Value / 100;
            
        //    IEnumerable<OperatorControl> checkedOperatorControls = _mainWindow.ControlPanel.OperatorControls.Where(cb => cb.IsChecked);
        //    IDictionary<Operator, double> operatorAndProbabilityMap =
        //        new DictionaryExt<Operator, double>(checkedOperatorControls.Select(opc => new KeyValuePair<Operator, double>(opc.Operator, opc.Probability)));

        //    Func<double> createConst = () => 
        //    {
        //        double d = _random.Next(_configuration.ConstantBounds);
        //        return Math.Abs(d - 0) < 0.01 ? 0.01 : d;
        //    };

        //    return FormulaTreeGenerator.Generate(operatorAndProbabilityMap, createConst, dimensionsCount, minimalDepth,
        //        _random, leafProbability, constantProbability);
        //}

        private bool _isAnimationStarted;

        private void StartStopAnimation()
        {
            _isAnimationStarted = !_isAnimationStarted;
            if (_isAnimationStarted)
            {
                StartAnimation();
            }
        }

        readonly Random _random = new Random();

        private async void StartAnimation()
        {
            FormulaRenderArguments formulaRenderArguments = UserInputFormulaRenderArguments;
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
            while (_isAnimationStarted)
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
            FormulaRenderArguments formulaRenderArguments = UserInputFormulaRenderArguments;
            formulaRenderArguments = getNextFormulaRenderArguments(formulaRenderArguments);
            _workflow.FormulaRenderArguments = formulaRenderArguments;
            _mainWindow.FormulaTexBox.Dispatcher.Invoke(() => _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString());
            await DrawImageAsync();
        }

        private async Task DrawImageAsync()
        {
            _mainWindow.Cursor = Cursors.Wait;

            ProgressObserver renderingProgressObserver = new ProgressObserver(
                p => _mainWindow.StatusPanel.Dispatcher.Invoke(() => _mainWindow.StatusPanel.RenderingProgress = p.Progress));

            FormulaRenderResult formulaRenderResult = await _workflow.RenderFormulaAsync(renderingProgressObserver);

            _wallpaperImage = new WallpaperImage(formulaRenderResult.Image.WidthInPixels, formulaRenderResult.Image.HeightInPixels);
            _wallpaperImage.Update(formulaRenderResult.Image);

            _mainWindow.WallpaperImage.Source = _wallpaperImage.Source;

            _mainWindow.StatusPanel.RenderedTime = formulaRenderResult.ElapsedTime;
            _mainWindow.Cursor = Cursors.Arrow;
        }

        private void SaveFormulaImage()
        {
            if (_wallpaperImage != null)
                _wallpaperImage.SaveToFile("c:/temp/wlp.png");
        }
    }
}

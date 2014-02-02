using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WallpaperGenerator.App.Core;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Utilities;
using WallpaperGenerator.Utilities.ProgressReporting;
using Size = WallpaperGenerator.Utilities.Size;

namespace WallpaperGenerator.App.Windows
{
    public class WallpaperGeneratorApplication : Application
    {        
        #region Fields

        private readonly FormulaRenderWorkflow _workflow;
        private readonly WindowsWallpaperFileManager _wallpaperFileManager;
        private readonly MainWindow _mainWindow;
        
        #endregion

        private FormulaRenderArguments UserInputFormulaRenderArguments
        {
            get
            {
                string formuleString = _mainWindow.FormulaTexBox.Dispatcher.Invoke(() => _mainWindow.FormulaTexBox.Text);
                FormulaRenderArguments args = formuleString != ""
                    ? FormulaRenderArguments.FromString(formuleString)
                    : null;
                if (args != null)
                {
                    args.ImageSize = _mainWindow.ControlPanel.ImageSizeControl.Size;
                }
                return args;
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
                
                _mainWindow.ControlPanel.SaveState(generationParams);

                return generationParams;
            }
        }
        
        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _workflow = new FormulaRenderWorkflow(new FormulaRenderArgumentsGenerationParams { PredefinedFormulaRenderingArgumentsEnabled = false },
                new Size(800, 800), s => new WindowsFormulaBitmap(s), Environment.ProcessorCount);
            _wallpaperFileManager = new WindowsWallpaperFileManager();
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };
            _mainWindow.ControlPanel.LoadState(_workflow.GenerationParams);
            _mainWindow.ControlPanel.ImageSizeControl.Size = _workflow.ImageSize;
            
            _mainWindow.ControlPanel.GenerateFormulaButton.Click += async (s, a) =>
            {
                if (_mainWindow.ControlPanel.RandomizeCheckBox.IsChecked.HasValue && _mainWindow.ControlPanel.RandomizeCheckBox.IsChecked.Value)
                {
                    FormulaRenderArgumentsGenerationParams randomParams = RandomizeFormulaRenderArgumentsGenerationParams(new FormulaRenderArgumentsGenerationParams());
                    _mainWindow.ControlPanel.LoadState(randomParams);
                }
                _workflow.GenerationParams = UserInputFormulaRenderArgumentsGenerationParams;
                FormulaRenderArguments formulaRenderArguments = _workflow.GenerateFormulaRenderArguments();
                _mainWindow.FormulaTexBox.Text = formulaRenderArguments.ToString();
                await DrawImageAsync();
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
                _workflow.ImageSize = _workflow.FormulaRenderArguments.ImageSize;
                await DrawImageAsync();
            };

            _mainWindow.ControlPanel.StartStopSmoothAnimationButton.Click += (s, a) => StartStopAnimation();

            _mainWindow.ControlPanel.SaveButton.Click += (s, a) => SaveFormulaImage();
        }

        private FormulaRenderArgumentsGenerationParams RandomizeFormulaRenderArgumentsGenerationParams(FormulaRenderArgumentsGenerationParams initParams)
        {
            FormulaRenderArgumentsGenerationParams randomParams = new FormulaRenderArgumentsGenerationParams();

            int dimensionsCount = _random.Next(initParams.DimensionCountBounds);
            randomParams.DimensionCountBounds = new Bounds<int>(dimensionsCount, dimensionsCount);

            int minimalDepth = _random.Next(initParams.MinimalDepthBounds);
            randomParams.MinimalDepthBounds = new Bounds<int>(minimalDepth, minimalDepth);
            
            double leafProbability = _random.Next(initParams.LeafProbabilityBounds);
            randomParams.LeafProbabilityBounds = new Bounds(leafProbability, leafProbability);

            double constantProbability = _random.Next(initParams.ConstantProbabilityBounds);
            randomParams.ConstantProbabilityBounds = new Bounds(constantProbability, constantProbability);

            double unaryVsBinaryOperatorsProbability = _random.Next(initParams.UnaryVsBinaryOperatorsProbabilityBounds);
            randomParams.UnaryVsBinaryOperatorsProbabilityBounds = new Bounds(unaryVsBinaryOperatorsProbability, unaryVsBinaryOperatorsProbability);
            
            return randomParams;
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
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

            WorkflowRenderResult result = await _workflow.RenderFormulaAsync(false, renderingProgressObserver);
            result.Bitmap.Update(result.FormulaRenderResult);
            _mainWindow.WallpaperImage.Source = (ImageSource)result.Bitmap.PlatformBitmap;

            _mainWindow.StatusPanel.RenderedTime = result.ElapsedTime;
            _mainWindow.Cursor = Cursors.Arrow;
        }

        private void SaveFormulaImage()
        {
            if (!_workflow.IsImageReady)
                return;

            _wallpaperFileManager.Save(_workflow.LastWorkflowRenderResult, true);
        }
    }
}

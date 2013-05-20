using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.MainWindowControls.ControlPanelControls;

namespace WallpaperGenerator
{
    public class WallpaperGeneratorApplication : Application
    {
        #region Fields

        private readonly Random _random = new Random();
        private readonly MainWindow _mainWindow;
        private readonly WallpaperImage _wallpaperImage;
        
        #endregion

        #region Properties

        public FormulaRenderingArguments FormulaRenderingArguments 
        {
            get { return FormulaRenderingArguments.FromString(_mainWindow.FormulaTexBox.Text); }
        }

        #endregion


        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _wallpaperImage = new WallpaperImage(1440, 1440);
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _mainWindow.ControlPanel.GenerateFormulaButton.Click += (s, a) =>
            {
                ColorTransformation colorTransformation = ColorTransformation.CreateRandomPolynomialColorTransformation(_random);
                FormulaTreeNode formulaTreeRoot = CreateRandomFormulaTreeRoot();
                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(formulaTreeRoot, colorTransformation);
                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
            };

            _mainWindow.ControlPanel.RenderFormulaButton.Click += (s, a) =>
                {
                    _mainWindow.Cursor = Cursors.Wait;

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();  

                    Range[] variableValuesRanges = CreateRandomVariableValuesRanges(FormulaRenderingArguments.FormulaTree, _wallpaperImage.WidthInPixels, _wallpaperImage.HeightInPixels).ToArray();
                    
                    RenderedFormulaImage renderedFormulaImage = FormulaRender.Render(
                        FormulaRenderingArguments.FormulaTree,
                        variableValuesRanges,
                        FormulaRenderingArguments.ColorTransformation, 
                        _wallpaperImage.WidthInPixels, _wallpaperImage.HeightInPixels);

                    _wallpaperImage.Update(renderedFormulaImage);
                    _mainWindow.WallpaperImage.Source = _wallpaperImage.Source;

                    stopwatch.Stop();
                    _mainWindow.StatusPanel.RenderedTime = stopwatch.Elapsed; 

                    _mainWindow.Cursor = Cursors.Arrow;
            };
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
        }

        private FormulaTreeNode CreateRandomFormulaTreeRoot()
        {
            int dimensionsCount = (int)_mainWindow.ControlPanel.DimensionsCountSlider.Value;
            int variablesCount = (int)_mainWindow.ControlPanel.VariablesCountSlider.Value;
            int constantsCount = (int)_mainWindow.ControlPanel.ConstantsCountSlider.Value;
            int unaryOperatorsCount = (int)_mainWindow.ControlPanel.UnaryOperatorsCountSlider.Value;
            IEnumerable<OperatorCheckBox> checkedOperatorCheckBoxes = _mainWindow.ControlPanel.OperatorCheckBoxes.Where(cb => cb.IsChecked == true);
            IEnumerable<Operator> operators = checkedOperatorCheckBoxes.Select(cb => cb.Operator);

            return FormulaTreeGenerator.CreateRandomFormulaTree(_random, dimensionsCount, variablesCount, constantsCount, unaryOperatorsCount, operators);
        }

        private IEnumerable<Range> CreateRandomVariableValuesRanges(FormulaTree formulaTree, int xRangeCount, int yRangeCount)
        {
            int dimensions = formulaTree.Variables.Length;
            return Enumerable.Repeat(1, dimensions).
                Select(i => CreateRandomVariableValuesRange(_random, i%2 == 0 ? xRangeCount : yRangeCount));
        }

        private static Range CreateRandomVariableValuesRange(Random random, int rangeCount)
        {
            return new Range(random.NextDouble()*random.Next(0, 10), 0.0000001 + random.NextDouble()/10, rangeCount);
        }
    }
}

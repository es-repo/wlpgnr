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
         
        #endregion

        #region Properties

        public FormulaRenderingArguments GetCurrentFormulaRenderingArguments()
        {
            return _mainWindow.FormulaTexBox.Text != "" 
                ? FormulaRenderingArguments.FromString(_mainWindow.FormulaTexBox.Text)
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
                
                VariableValuesRangesFor2DProjection variableValuesRanges =
                    CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length, currentFormulaRenderingArguments);

                ColorTransformation colorTransformation = CreateRandomColorTransformation();
                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(formulaTree, variableValuesRanges, colorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
            };

            _mainWindow.ControlPanel.ChangeRangesButton.Click += (s, a) =>
            {
                FormulaRenderingArguments currentFormulaRenderingArguments = GetCurrentFormulaRenderingArguments();
                VariableValuesRangesFor2DProjection variableValuesRanges = CreateRandomVariableValuesRangesFor2DProjection(
                        currentFormulaRenderingArguments.FormulaTree.Variables.Length, currentFormulaRenderingArguments);

                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(
                    currentFormulaRenderingArguments.FormulaTree,
                    variableValuesRanges,
                    currentFormulaRenderingArguments.ColorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
                RenderFormula();
            };

            _mainWindow.ControlPanel.ChangeColorButton.Click += (s, a) =>
            {
                FormulaRenderingArguments currentFormulaRenderingArguments = GetCurrentFormulaRenderingArguments(); 
                ColorTransformation colorTransformation = CreateRandomColorTransformation();
                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(
                    currentFormulaRenderingArguments.FormulaTree, 
                    currentFormulaRenderingArguments.VariableValuesRanges, 
                    colorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
                RenderFormula();
            };
            
            _mainWindow.ControlPanel.RenderFormulaButton.Click += (s, a) => RenderFormula();
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
            IDictionary<int, double> arityOpNodeProbabilityMap = new Dictionary<int, double>();
            foreach (var e in _mainWindow.ControlPanel.OpNodesProbabilities)
            {
                arityOpNodeProbabilityMap.Add(e.Key, e.Value.Value);
            }
            IEnumerable<OperatorCheckBox> checkedOperatorCheckBoxes = _mainWindow.ControlPanel.OperatorCheckBoxes.Where(cb => cb.IsChecked == true);
            IEnumerable<Operator> operators = checkedOperatorCheckBoxes.Select(cb => cb.Operator);

            Func<double> createConst = () => 
            {
                double d = Math.Round(_random.NextDouble() * 50 - 25, 2);
                return Math.Abs(d - 0) < 0.01 ? 0.01 : d;
            };

            return FormulaTreeGenerator.Generate(operators, createConst, dimensionsCount, minimalDepth, 
                _random, constantProbability, arityOpNodeProbabilityMap);
        }

        private VariableValuesRangesFor2DProjection CreateRandomVariableValuesRangesFor2DProjection(int variablesCount, 
            FormulaRenderingArguments currentFormulaRenderingArguments)
        {
            int xRangeCount = currentFormulaRenderingArguments != null
                    ? currentFormulaRenderingArguments.VariableValuesRanges.XCount
                    : Configuration.DefaultImageWidth;

            int yRangeCount = currentFormulaRenderingArguments != null
                ? currentFormulaRenderingArguments.VariableValuesRanges.YCount
                : Configuration.DefaultImageWidth;  
            
            return VariableValuesRangesFor2DProjection.CreateRandom(_random, variablesCount,
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

        private void RenderFormula()
        {
            _mainWindow.Cursor = Cursors.Wait;

            FormulaRenderingArguments formulaRenderingArguments = GetCurrentFormulaRenderingArguments();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            RenderedFormulaImage renderedFormulaImage = FormulaRender.Render(
                formulaRenderingArguments.FormulaTree,
                formulaRenderingArguments.VariableValuesRanges,
                formulaRenderingArguments.ColorTransformation);

            WallpaperImage wallpaperImage =
                new WallpaperImage(renderedFormulaImage.WidthInPixels, renderedFormulaImage.HeightInPixels);
            wallpaperImage.Update(renderedFormulaImage);

            _mainWindow.WallpaperImage.Source = wallpaperImage.Source;

            stopwatch.Stop();

            _mainWindow.StatusPanel.RenderedTime = stopwatch.Elapsed;
            _mainWindow.Cursor = Cursors.Arrow;
        }
    }
}

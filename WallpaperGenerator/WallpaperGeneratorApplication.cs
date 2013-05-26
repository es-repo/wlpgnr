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

        public FormulaRenderingArguments GetCurrentFormulaRenderingArguments()
        {
            return FormulaRenderingArguments.FromString(_mainWindow.FormulaTexBox.Text);
        }

        #endregion


        #region Constructors

        public WallpaperGeneratorApplication()
        {
            MathLibrary.Init(Configuration.PrecalculatedSinusesCount);

            _wallpaperImage = new WallpaperImage(Configuration.ImageWidth, Configuration.ImageHeight);
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _mainWindow.ControlPanel.GenerateFormulaButton.Click += (s, a) =>
            {
                FormulaTreeNode formulaTreeRoot = CreateRandomFormulaTreeRoot();
                FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
                VariableValuesRangesFor2DProjection variableValuesRanges = CreateRandomVariableValuesRangesFor2DProjection(formulaTree.Variables.Length);
                ColorTransformation colorTransformation = CreateRandomColorTransformation();
                FormulaRenderingArguments formulaRenderingArguments = new FormulaRenderingArguments(formulaTree, variableValuesRanges, colorTransformation);

                _mainWindow.FormulaTexBox.Text = formulaRenderingArguments.ToString();
            };

            _mainWindow.ControlPanel.ChangeRangesButton.Click += (s, a) =>
            {
                FormulaRenderingArguments currentFormulaRenderingArguments = GetCurrentFormulaRenderingArguments();
                VariableValuesRangesFor2DProjection variableValuesRanges = 
                    CreateRandomVariableValuesRangesFor2DProjection(currentFormulaRenderingArguments.FormulaTree.Variables.Length);
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

        private VariableValuesRangesFor2DProjection CreateRandomVariableValuesRangesFor2DProjection(int variablesCount)
        {
            return VariableValuesRangesFor2DProjection.CreateRandom(_random, variablesCount,
                Configuration.ImageWidth, Configuration.ImageHeight,
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

            _wallpaperImage.Update(renderedFormulaImage);
            _mainWindow.WallpaperImage.Source = _wallpaperImage.Source;

            stopwatch.Stop();

            _mainWindow.StatusPanel.RenderedTime = stopwatch.Elapsed;
            _mainWindow.Cursor = Cursors.Arrow;
        }
    }
}

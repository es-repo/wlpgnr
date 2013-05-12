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

        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _wallpaperImage = new WallpaperImage(700, 700);
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _mainWindow.ControlPanel.GenerateFormulaButton.Click += (s, a) =>
            {                
                int dimensionsCount = (int) _mainWindow.ControlPanel.DimensionsCountSlider.Value;
                int variablesCount = (int)_mainWindow.ControlPanel.VariablesCountSlider.Value;
                int constantsCount = (int)_mainWindow.ControlPanel.ConstantsCountSlider.Value;
                int unaryOperatorsCount = (int)_mainWindow.ControlPanel.UnaryOperatorsCountSlider.Value;
                IEnumerable<OperatorCheckBox> checkedOperatorCheckBoxes = _mainWindow.ControlPanel.OperatorCheckBoxes.Where(cb => cb.IsChecked == true);
                IEnumerable<Operator> operators = checkedOperatorCheckBoxes.Select(cb => cb.Operator);

                FormulaTreeNode formulaTree = FormulaTreeGenerator.CreateRandomFormulaTree(_random,
                    dimensionsCount, variablesCount, constantsCount, unaryOperatorsCount, operators);
                string formula = FormulaTreeSerializer.Serialize(formulaTree, new FormulaTreeSerializationOptions { WithIndentation = true });
                _mainWindow.FormulaTexBox.Text = formula;
            };

            _mainWindow.ControlPanel.RenderFormulaButton.Click += (s, a) =>
                {
                    _mainWindow.Cursor = Cursors.Wait;

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();  

                    string formula = _mainWindow.FormulaTexBox.Text;
                    FormulaTreeNode formulaTreeRoot = FormulaTreeSerializer.Deserialize(formula);
                    FormulaTree formulaTree = new FormulaTree(formulaTreeRoot);
                    ColorTransformation colorTransformation = GetColorTransformation();
                    Range[] variableValuesRanges = GetRandomVariableValuesRanges(formulaTree, _wallpaperImage.WidthInPixels, _wallpaperImage.HeightInPixels).ToArray();
                    const double colorDispersionCoefficient = 0.5;
                    RenderedFormulaImage renderedFormulaImage = FormulaRender.Render(formulaTree, variableValuesRanges, colorTransformation,
                        colorDispersionCoefficient, _wallpaperImage.WidthInPixels, _wallpaperImage.HeightInPixels);
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

        private ColorTransformation GetColorTransformation()
        {
            double ra, rb, rc;
            GetRandomPolinomCoefficients(_random, out ra, out rb, out rc);

            double ga, gb, gc;
            GetRandomPolinomCoefficients(_random, out ga, out gb, out gc);

            double ba, bb, bc;
            GetRandomPolinomCoefficients(_random, out ba, out bb, out bc);

            return ColorTransformation.CreatePolynomialColorTransformation(ra, rb, rc, ga, gb, gc, ba, bb, bc);
        }

        private static void GetRandomPolinomCoefficients(Random random, out double a, out double b, out double c)
        {
            const int minValue = -10;
            const int maxValue = 10;
            a = random.Next(minValue, maxValue);
            b = random.Next(minValue, maxValue);
            c = random.Next(minValue, maxValue);
        }

        private IEnumerable<Range> GetRandomVariableValuesRanges(FormulaTree formulaTree, int xRangeCount, int yRangeCount)
        {
            int dimensions = formulaTree.Variables.Length;
            return Enumerable.Repeat(1, dimensions).
                Select(i => GetRandomVariableValuesRange(_random, i%2 == 0 ? xRangeCount : yRangeCount));
        }

        private static Range GetRandomVariableValuesRange(Random random, int rangeCount)
        {
            return new Range(random.NextDouble()*random.Next(0, 10), 0.0000001 + random.NextDouble()/10, rangeCount);
        }
    }
}

using System.Collections.Generic;
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

        private readonly MainWindow _mainWindow;
        private readonly WallpaperImage _wallpaperImage;
        
        #endregion

        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _wallpaperImage = new WallpaperImage(400, 400);
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _mainWindow.ControlPanel.GenerateFormulaButton.Click += (s, a) =>
            {
                IEnumerable<OperatorCheckBox> checkedOperatorCheckBoxes = _mainWindow.ControlPanel.OperatorCheckBoxes.Where(cb => cb.IsChecked == true);
                IEnumerable<Operator> operators = checkedOperatorCheckBoxes.Select(cb => cb.Operator);
                FormulaTreeNode formulaTree = FormulaTreeGenerator.CreateRandomFormulaTree(2, 2, 0, operators);
                string formula = FormulaTreeSerializer.Serialize(formulaTree, new FormulaTreeSerializationOptions { WithIndentation = true });
                _mainWindow.FormulaTexBox.Text = formula;
            };

            _mainWindow.ControlPanel.RenderFormulaButton.Click += (s, a) =>
                {
                    _mainWindow.Cursor = Cursors.Wait;     

                    string formula = _mainWindow.FormulaTexBox.Text;
                    FormulaTreeNode formulaTree = FormulaTreeSerializer.Deserialize(formula);
                    RenderedFormulaImage renderedFormulaImage = FormulaRender.Render(formulaTree, _wallpaperImage.WidthInPixels, _wallpaperImage.HeightInPixels);
                    _wallpaperImage.Update(renderedFormulaImage);
                    _mainWindow.WallpaperImage.Source = _wallpaperImage.Source;

                    _mainWindow.Cursor = Cursors.Arrow;
            };
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
        }
    }

}

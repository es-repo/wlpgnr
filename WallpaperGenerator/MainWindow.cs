using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.MainWindowControls;

namespace WallpaperGenerator
{
    public class MainWindow : Window
    {
        #region Properties 

        public ControlPanel ControlPanel { get; private set; }

        public Image WallpaperImage { get; private set; }

        public TextBox FormulaTexBox { get; private set; }

        #endregion

        #region Constructors

        public MainWindow()
        {
            Title = "Wallpaper Generator";

            StackPanel mainPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            StackPanel formulaAndImagePanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Panel formulaPanel = CreateFormulaPanel();
            formulaAndImagePanel.Children.Add(formulaPanel);

            Panel imagePanel = CreateImagePanel();
            formulaAndImagePanel.Children.Add(imagePanel);

            mainPanel.Children.Add(formulaAndImagePanel);

            ControlPanel = CreateControlPanel();
            mainPanel.Children.Add(ControlPanel);
            
            Content = mainPanel;
        }

        #endregion

        private Panel CreateFormulaPanel()
        {
            StackPanel formulaPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Top,
            };

            FormulaTexBox = new TextBox
            {
                AcceptsReturn = true,
                Height = 200,
                Width = 800
            };

            formulaPanel.Children.Add(FormulaTexBox);
            return formulaPanel;
        }

        private Panel CreateImagePanel()
        {
            StackPanel imagePanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            
            WallpaperImage = new Image
            {
                Stretch = Stretch.Fill
            };

            imagePanel.Children.Add(WallpaperImage);
            return imagePanel;
        }

        private ControlPanel CreateControlPanel()
        {
            ControlPanel controlPanel = new ControlPanel();
            controlPanel.FormulaGenerated += (s, a) =>
            {
                string formula = FormulaTreeSerializer.Serialize(a.Object, new FormulaTreeSerializationOptions { WithIndentation = true } );
                FormulaTexBox.Text = formula;
            };
            return controlPanel;
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WallpaperGenerator
{
    public class MainWindow : Window
    {
        #region Properties 

        public Image WallpaperImage { get; private set; }

        #endregion

        #region Constructors

        public MainWindow()
        {
            Title = "Wallpaper Generator";

            WallpaperImage = new Image
            {
                Stretch = Stretch.None, 
                Margin = new Thickness(20)
            };

            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Height = 800,
                Width = 800
            };

            stackPanel.Children.Add(WallpaperImage);
            Content = stackPanel;
        }

        #endregion

    }
}

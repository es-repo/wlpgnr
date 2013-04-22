using System;
using System.Windows;
using System.Windows.Threading;

namespace WallpaperGenerator
{
    public class WallpaperGeneratorApplication : Application
    {
        #region Fields

        private readonly MainWindow _mainWindow;
        private readonly WallpaperImage _wallpaperImage;
        private readonly DispatcherTimer _dispatcherTimer;

        #endregion

        #region Constructors

        public WallpaperGeneratorApplication()
        {
            _wallpaperImage = new WallpaperImage();
            _mainWindow = new MainWindow { WindowState = WindowState.Maximized };

            _dispatcherTimer = new DispatcherTimer
            {
                IsEnabled = true,
                Interval = new TimeSpan(0, 0, 1)
            };

            _dispatcherTimer.Tick += (s, e) =>
            {
                _wallpaperImage.Update();
                _mainWindow.WallpaperImage.Source = _wallpaperImage.Source;
            };
        }

        #endregion

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mainWindow.Show();
            _dispatcherTimer.Start();
        }
    }

}

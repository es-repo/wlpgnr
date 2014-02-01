using System;
using System.Windows;

namespace WallpaperGenerator.App.Windows
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            WallpaperGeneratorApplication app = new WallpaperGeneratorApplication();
            app.DispatcherUnhandledException += (s, a) =>
            {
                MessageBox.Show(a.Exception.Message);
                a.Handled = true;
            };
            app.Run();
        }
    }
}

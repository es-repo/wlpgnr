using System;

namespace WallpaperGenerator
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var app = new WallpaperGeneratorApplication();
            app.Run();
        }
    }
}

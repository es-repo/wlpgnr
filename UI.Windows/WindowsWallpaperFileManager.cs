using System.IO;
using System.Windows.Media.Imaging;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.UI.Shared;

namespace WallpaperGenerator.UI.Windows
{
    public class WindowsWallpaperFileManager : WallpaperFileManager
    {
        public WindowsWallpaperFileManager()
            : base("c://Temp")
        {
        }

        protected override void WriteImageAsPng(FormulaRenderResult image, Stream stream)
        {
            WriteableBitmap bitmap = image.ToBitmap();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(stream);
        }
    }
}

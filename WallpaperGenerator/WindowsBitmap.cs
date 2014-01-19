using System.IO;
using System.Windows.Media.Imaging;
using WallpaperGenerator.UI.Core;

namespace WallpaperGenerator.UI.Windows
{
    public class WindowsBitmap : BaseBitmap
    {
        private readonly WriteableBitmap _bitmap;

        public WindowsBitmap(WriteableBitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public override void WriteAsPng(Stream stream)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(_bitmap));
            encoder.Save(stream);
        }
    }
}
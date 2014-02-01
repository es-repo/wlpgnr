using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Windows
{
    public static class FormulaRenderResultExtensions
    {
        public static WriteableBitmap ToBitmap(this FormulaRenderResult image)
        {
            WriteableBitmap bitmap = new WriteableBitmap(image.Size.Width, image.Size.Height, 96, 96, PixelFormats.Bgra32, null);
            Int32Rect rect = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int stride = bitmap.PixelWidth * bytesPerPixel;

            byte[] colors = new byte[image.RedChannel.Length * 4];
            for (int i = 0, j = 0; i < image.RedChannel.Length; i++, j += 4)
            {
                colors[j] = image.BlueChannel[i];
                colors[j + 1] = image.GreenChannel[i];
                colors[j + 2] = image.RedChannel[i];
                colors[j + 3] = 255;
            }
            bitmap.WritePixels(rect, colors, stride, 0);
            return bitmap;
        }
    }
}

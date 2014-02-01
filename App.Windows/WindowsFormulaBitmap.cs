using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WallpaperGenerator.App.Core;
using WallpaperGenerator.FormulaRendering;
using Size = WallpaperGenerator.Utilities.Size;

namespace WallpaperGenerator.App.Windows
{
    public class WindowsFormulaBitmap : FormulaBitmap
    {
        private readonly WriteableBitmap _bitmap;
        private readonly byte[] _pixelsBuffer;
        private readonly int _stride;
        private readonly Int32Rect _rect;

        public WindowsFormulaBitmap(Size size) : base(size)
        {
            PlatformBitmap = _bitmap = new WriteableBitmap(size.Width, size.Height, 96, 96, PixelFormats.Bgra32, null);
            int bytesPerPixel = (_bitmap.Format.BitsPerPixel + 7) / 8;
            _stride = _bitmap.PixelWidth * bytesPerPixel;
            _rect = new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight);
            _pixelsBuffer = new byte[size.Square * 4];
        }

        public override void Update(FormulaRenderResult formulaRenderResult)
        {
            for (int i = 0, j = 0; i < formulaRenderResult.RedChannel.Length; i++, j += 4)
            {
                _pixelsBuffer[j] = formulaRenderResult.BlueChannel[i];
                _pixelsBuffer[j + 1] = formulaRenderResult.GreenChannel[i];
                _pixelsBuffer[j + 2] = formulaRenderResult.RedChannel[i];
                _pixelsBuffer[j + 3] = 255;
            }

            _bitmap.WritePixels(_rect, _pixelsBuffer, _stride, 0);
        }

        public override void WriteAsPng(Stream stream)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(_bitmap));
            encoder.Save(stream);
        }
    }
}

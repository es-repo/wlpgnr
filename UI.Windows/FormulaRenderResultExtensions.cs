using System;
using System.Windows;
using System.Windows.Media.Imaging;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Windows
{
    public static class FormulaRenderResultExtensions
    {
<<<<<<< HEAD
        public static WriteableBitmap ToBitmap(this FormulaRenderResult formulaRenderResult)
        {
            WriteableBitmap bitmap = new WriteableBitmap(formulaRenderResult.Size.Width, formulaRenderResult.Size.Height, 96, 96, PixelFormats.Bgra32, null);
            Int32Rect rect = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int stride = bitmap.PixelWidth * bytesPerPixel;

            byte[] colors = new byte[formulaRenderResult.RedChannel.Length * 4];
            for (int i = 0, j = 0; i < formulaRenderResult.RedChannel.Length; i++, j += 4)
            {
                colors[j] = formulaRenderResult.BlueChannel[i];
                colors[j + 1] = formulaRenderResult.GreenChannel[i];
                colors[j + 2] = formulaRenderResult.RedChannel[i];
                colors[j + 3] = 255;
=======
        public static WriteableBitmap WriteToBitmap(this FormulaRenderResult formulaRenderResult, WriteableBitmap bitmap, byte[] pixelsBuffer)
        {
            if (bitmap.PixelWidth * bitmap.PixelHeight != formulaRenderResult.Size.Width * formulaRenderResult.Size.Width)
                throw new ArgumentException("Bitmap size isn't equal to rendered  formula size.", "bitmap");
            
            if (pixelsBuffer.Length / 4 != formulaRenderResult.Size.Width * formulaRenderResult.Size.Width)
                throw new ArgumentException("Pixels buffer size isn't equal to rendered formula size.", "pixelsBuffer");
            
            int bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            int stride = bitmap.PixelWidth * bytesPerPixel;

            for (int i = 0, j = 0; i < formulaRenderResult.RedChannel.Length; i++, j += 4)
            {
                pixelsBuffer[j] = formulaRenderResult.BlueChannel[i];
                pixelsBuffer[j + 1] = formulaRenderResult.GreenChannel[i];
                pixelsBuffer[j + 2] = formulaRenderResult.RedChannel[i];
                pixelsBuffer[j + 3] = 255;
>>>>>>> 7ab49bc415cc96e05f4bfbd380c5d7c2d06a7608
            }

            Int32Rect rect = new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight);
            bitmap.WritePixels(rect, pixelsBuffer, stride, 0);
            return bitmap;
        }
    }
}

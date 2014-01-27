using Android.Graphics;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Android
{
    public static class RenderedFormulaImageExtensions
    {
        public static Bitmap ToBitmap(this RenderedFormulaImage image)
        {
            int length = image.RedChannel.Length;
            int[] pixels = new int[length];
            for (int i = 0; i < length; i++)
                pixels[i] = Color.Argb(255, image.RedChannel[i], image.GreenChannel[i], image.BlueChannel[i]);

            return Bitmap.CreateBitmap(pixels, image.Size.Width, image.Size.Height, Bitmap.Config.Argb8888);
        }
    }
}
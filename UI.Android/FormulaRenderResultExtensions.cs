using Android.Graphics;
using WallpaperGenerator.FormulaRendering;

namespace WallpaperGenerator.UI.Android
{
    public static class FormulaRenderResultExtensions
    {
        public static Bitmap ToBitmap(this FormulaRenderResult formulaRenderResult)
        {
            int length = formulaRenderResult.RedChannel.Length;
            int[] pixels = new int[length];
            for (int i = 0; i < length; i++)
                pixels[i] = Color.Argb(255, formulaRenderResult.RedChannel[i], formulaRenderResult.GreenChannel[i], formulaRenderResult.BlueChannel[i]);

            
            return Bitmap.CreateBitmap(pixels, formulaRenderResult.Size.Width, formulaRenderResult.Size.Height, Bitmap.Config.Argb8888);
        }
    }
}
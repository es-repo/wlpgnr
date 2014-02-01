using System.IO;
using Android.Graphics;
using WallpaperGenerator.App.Core;
using WallpaperGenerator.FormulaRendering;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.App.Android
{
    public class AndroidFormulaBitmap : FormulaBitmap
    {
        private readonly int[] _pixelsBuffer;

        public Bitmap _bitmap { get; private set; }

        public AndroidFormulaBitmap(Size size) : base (size)
        {
            _pixelsBuffer = new int[size.Square];
            Bitmap immutableBitmap = Bitmap.CreateBitmap(size.Width, size.Height, Bitmap.Config.Argb8888);
            PlatformBitmap = _bitmap = immutableBitmap.Copy(Bitmap.Config.Argb8888, true);
            immutableBitmap.Recycle();
        }

        public override void Update(FormulaRenderResult formulaRenderResult)
        {
            for (int i = 0; i < formulaRenderResult.Size.Square; i++)
                _pixelsBuffer[i] = Color.Argb(255, formulaRenderResult.RedChannel[i], formulaRenderResult.GreenChannel[i], formulaRenderResult.BlueChannel[i]);
           
            _bitmap.SetPixels(_pixelsBuffer, 0, Size.Width, 0, 0, Size.Width, Size.Height);
        }

        public override void WriteAsPng(Stream stream)
        {
            _bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
        }
    }
}
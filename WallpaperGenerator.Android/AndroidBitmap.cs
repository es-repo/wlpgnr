using Android.Graphics;
using WallpaperGenerator.UI.Core;
using System.IO;

namespace WallpaperGenerator.UI.Android
{
    public class AndroidBitmap : BaseBitmap
    {
        private readonly Bitmap _bitmap;

        public AndroidBitmap(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public override void WriteAsPng(Stream stream)
        {
            _bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
        }
    }
}
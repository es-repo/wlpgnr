using System.IO;

namespace WallpaperGenerator.UI.Core
{
    public abstract class BaseBitmap
    {
        public abstract void WriteAsPng(Stream stream);
    }
}

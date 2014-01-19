using System.IO;

namespace WallpaperGenerator.UI.Core
{
    public abstract class BitMap
    {
        public abstract void WriteAsPng(Stream stream);
    }
}

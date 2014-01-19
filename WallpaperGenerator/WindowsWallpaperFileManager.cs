using System.Windows.Media.Imaging;
using UI.Shared;

namespace WallpaperGenerator.UI.Windows
{
    public class WindowsWallpaperFileManager : WallpaperFileManager
    {
        public WindowsWallpaperFileManager()
            : base("c://Temp")
        {
        }

        public virtual string Save(WriteableBitmap bitmap)
        {
            return Save(new WindowsBitmap(bitmap));
        }
    }
}

using System.Globalization;

namespace WallpaperGenerator.Utilities
{
    public static class ObjectExtensions
    {
        public static string ToInvariantString(this double d)
        {
            return d.ToString(CultureInfo.InvariantCulture);
        }
    }
}

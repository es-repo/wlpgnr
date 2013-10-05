using System.Globalization;

namespace WallpaperGenerator.Utilities
{
    public static class ObjectExtensions
    {
        public static string ToInvariantString(this double o)
        {
            return o.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this int o)
        {
            return o.ToString(CultureInfo.InvariantCulture);
        }
    }
}

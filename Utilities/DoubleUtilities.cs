using System.Globalization;

namespace WallpaperGenerator.Utilities
{
    public static class DoubleUtilities
    {
        public static double ParseInvariant(string value)
        {
            return double.Parse(value, CultureInfo.InvariantCulture);
        }
    }
}

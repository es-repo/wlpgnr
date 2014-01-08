using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.Core
{
    public class Configuration
    {
        public static readonly Bounds<int> DimensionCountBounds = new Bounds<int>(4, 20);
        public static readonly Bounds<int> MinimalDepthBounds = new Bounds<int>(7, 12);
        public static readonly Bounds ConstantBounds = new Bounds(-10, 10);
        public static readonly Bounds ConstantProbabilityBounds = new Bounds(0, 1);
        public static readonly Bounds LeafProbabilityBounds = new Bounds(0, 0.5);
        public static readonly Bounds RangeBounds = new Bounds(-50, 50);
        public static readonly Bounds ColorChannelPolinomialTransformationCoefficientBounds = new Bounds(-50, 50);
        public static readonly double ColorChannelZeroProbabilty = 0.1;
    }
}

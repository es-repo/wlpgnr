using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderConfiguration
    {
        public static Bounds<int> DimensionCountBounds = new Bounds<int>(4, 20);
        public static Bounds<int> MinimalDepthBounds = new Bounds<int>(8, 14);
        public static Bounds ConstantBounds = new Bounds(-10, 10);
        public static Bounds ConstantProbabilityBounds = new Bounds(0, 1);
        public static Bounds LeafProbabilityBounds = new Bounds(0, 0.5);
        public static Bounds RangeBounds = new Bounds(-50, 50);
        public static Bounds ColorChannelPolinomialTransformationCoefficientBounds = new Bounds(-50, 50);
        public static double ColorChannelZeroProbabilty = 0.1;
    }
}

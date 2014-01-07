namespace WallpaperGenerator.Core
{
    public class Configuration
    {
        public const int DimensionCountLowBound = 4;
        public const int DimensionCountHighBound = 20;
        public const int MinimalDepthLowBound = 10;
        public const int MinimalDepthHighBound = 20;
        public const double ConstantLowBound = -25;
        public const double ConstantHighBound = 25;
        public const double ConstantProbabilityLowBound = 0;
        public const double ConstantProbabilityHighBound = 1;
        public const double LeafProbabilityLowBound = 0;
        public const double LeafProbabilityHighBound = 1;
        public const int RangeLowBound = -50;
        public const int RangeHighBound = 50;
        public const int ColorChannelPolinomialTransformationCoefficientLowBound = -50;
        public const int ColorChannelPolinomialTransformationCoefficientHighBound = 50;
        public const double ColorChannelZeroProbabilty = 0.1;
    }
}

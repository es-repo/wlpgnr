using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderArgumentsGenerationParams
    {
        public int WidthInPixels = 720;
        public int HeightInPixels = 1280;
        public Bounds<int> DimensionCountBounds = new Bounds<int>(4, 20);
        public Bounds<int> MinimalDepthBounds = new Bounds<int>(8, 14);
        public Bounds ConstantBounds = new Bounds(-10, 10);
        public Bounds ConstantProbabilityBounds = new Bounds(0, 1);
        public Bounds LeafProbabilityBounds = new Bounds(0, 0.5);
        public Bounds RangeBounds = new Bounds(-50, 50);
        public Bounds ColorChannelPolinomialTransformationCoefficientBounds = new Bounds(-50, 50);
        public double ColorChannelZeroProbabilty = 0.1;
        public Operator[] Operators = { OperatorsLibrary.Sum, OperatorsLibrary.Sub, OperatorsLibrary.Ln, OperatorsLibrary.Sin, 
                                       OperatorsLibrary.Max, OperatorsLibrary.Mul, OperatorsLibrary.Cbrt, OperatorsLibrary.Pow3 };
    }
}

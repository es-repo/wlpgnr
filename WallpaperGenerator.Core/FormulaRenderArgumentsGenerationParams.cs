using System.Collections.Generic;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderArgumentsGenerationParams
    {
        public int WidthInPixels = 360;
        public int HeightInPixels = 640;
        public Bounds<int> DimensionCountBounds = new Bounds<int>(4, 20);
        public Bounds<int> MinimalDepthBounds = new Bounds<int>(8, 14);
        public Bounds ConstantBounds = new Bounds(-10, 10);
        public Bounds ConstantProbabilityBounds = new Bounds(0, 1);
        public Bounds LeafProbabilityBounds = new Bounds(0, 0.5);
        public Bounds RangeBounds = new Bounds(-40, 40);
        public Bounds ColorChannelPolinomialTransformationCoefficientBounds = new Bounds(-10, 10);
        public double ColorChannelZeroProbabilty = 0.1;

        public IDictionary<Operator, double> OperatorAndMaxProbabilityMap = new Dictionary<Operator, double>
        {
            {OperatorsLibrary.Sum, 1},
            {OperatorsLibrary.Sub, 1},
            {OperatorsLibrary.Mul, 0.3},
            {OperatorsLibrary.Div, 0.2},
            {OperatorsLibrary.Max, 0.3},
            {OperatorsLibrary.Sin, 1},
            //{OperatorsLibrary.Cos, 1},
            {OperatorsLibrary.Ln, 1},
            {OperatorsLibrary.Cbrt, 1},
        }; 
    }
}

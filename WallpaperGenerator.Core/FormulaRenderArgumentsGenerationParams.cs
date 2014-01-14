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
        public Bounds<int> MinimalDepthBounds = new Bounds<int>(8, 13);
        public Bounds ConstantBounds = new Bounds(-10, 10);
        public Bounds ConstantProbabilityBounds = new Bounds(0, 0.5);
        public Bounds LeafProbabilityBounds = new Bounds(0, 0.25);
        public Bounds RangeBounds = new Bounds(-40, 40);
        public Bounds ColorChannelPolinomialTransformationCoefficientBounds = new Bounds(-10, 10);
        public double ColorChannelZeroProbabilty = 0.1;

        public IDictionary<Operator, double> OperatorAndMaxProbabilityMap = new Dictionary<Operator, double>
        {
            {OperatorsLibrary.Sum, 1},
            {OperatorsLibrary.Sub, 1},
            //{OperatorsLibrary.Mul, 0.2},
            //{OperatorsLibrary.Div, 0.1},
            //{OperatorsLibrary.Max, 0.2},
            //{OperatorsLibrary.Pow, 0.2},

            //{OperatorsLibrary.Abs, 0.3},
            {OperatorsLibrary.Sin, 0.5},
            {OperatorsLibrary.Cos, 0.5},
            //{OperatorsLibrary.Atan, 1},
            {OperatorsLibrary.Ln, 0.5},
            {OperatorsLibrary.Sqrt,0.2},
            {OperatorsLibrary.Cbrt,0.2},
            //{OperatorsLibrary.Pow2, 1},
            //{OperatorsLibrary.Pow3, 1},
            
        }; 
    }
}

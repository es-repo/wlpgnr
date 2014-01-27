using System.Collections.Generic;
using WallpaperGenerator.Formulas;
using WallpaperGenerator.Formulas.Operators;
using WallpaperGenerator.Utilities;

namespace WallpaperGenerator.UI.Core
{
    public class FormulaRenderArgumentsGenerationParams
    {
        public Bounds<int> DimensionCountBounds = new Bounds<int>(3, 15);
        public Bounds<int> MinimalDepthBounds = new Bounds<int>(10, 13);
        public Bounds ConstantBounds = new Bounds(-10, 10);
        public Bounds ConstantProbabilityBounds = new Bounds(0, 1);
        public Bounds LeafProbabilityBounds = new Bounds(0, 0.3);
        public Bounds RangeBounds = new Bounds(-40, 40);
        public Bounds ColorChannelPolinomialTransformationCoefficientBounds = new Bounds(-10, 10);
        public double ColorChannelZeroProbabilty = 0.2;
        public Bounds UnaryVsBinaryOperatorsProbabilityBounds = new Bounds(0.4, 0.70);

        public IDictionary<Operator, Bounds> OperatorAndMaxProbabilityBoundsMap = new Dictionary<Operator, Bounds>
        {
            {OperatorsLibrary.Sum, new Bounds(0.25, 1)},
            {OperatorsLibrary.Sub, new Bounds(0.25, 1)},
            {OperatorsLibrary.Mul, new Bounds(0, 0.25)},
            //{OperatorsLibrary.Div, new Bounds(0, 0.5)},
            {OperatorsLibrary.Max, new Bounds(0, 0.15)},
            {OperatorsLibrary.Pow, new Bounds(0, 0.5)},

            {OperatorsLibrary.Abs, new Bounds(0, 0.15)},
            {OperatorsLibrary.Sin, new Bounds(0.15, 1)},
            {OperatorsLibrary.Cos, new Bounds(0, 1)},
            {OperatorsLibrary.Atan, new Bounds(0, 0.25)},
            {OperatorsLibrary.Ln, new Bounds(0, 0.5)},
            {OperatorsLibrary.Sqrt, new Bounds(0, 1)},
            {OperatorsLibrary.Cbrt, new Bounds(0, 1)},
            {OperatorsLibrary.Pow2, new Bounds(0, 0.5)},
            {OperatorsLibrary.Pow3, new Bounds(0, 0.5)},
        };

        public Operator[] ObligatoryOperators = {OperatorsLibrary.Sin, OperatorsLibrary.Sum};
    }
}

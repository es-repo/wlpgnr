using System.Globalization;

namespace WallpaperGenerator.Formulas.Operators
{
    public sealed class Constant : ZeroArityOperator
    {
        public double Value { get; private set; }

        public Constant(double value)
            : this(value, value.ToString(CultureInfo. InvariantCulture)) 
        {
        }

        public Constant(double value, string name)
            : base(name)
        {
            Value = value;
        }

        protected override double EvaluateCore(params double[] operands)
        {
            return Value;
        }
    }
}

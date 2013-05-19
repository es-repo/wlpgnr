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

        public override double Evaluate(double op1, double op2, double op3, double op4)
        {
            return Value;
        }
    }
}

using System;
using System.Globalization;

namespace WallpaperGenerator.Formulas.Operators
{
    public sealed class Constant : ZeroArityOperator
    {
        public Constant(double value)
            : this(value, value.ToString(CultureInfo. InvariantCulture)) 
        {
        }

        public Constant(double value, string name)
            : base(name)
        {
            Value = value;
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            throw new InvalidOperationException();
        }

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            return () => Value;
        }
    }
}

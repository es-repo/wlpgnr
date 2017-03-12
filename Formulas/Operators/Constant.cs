using System;
using System.Globalization;
using System.Linq.Expressions;

namespace WallpaperGenerator.Formulas.Operators
{
    public sealed class Constant : ZeroArityOperator
    {
        public Constant(double value)
            : base(value.ToString("R", CultureInfo.InvariantCulture), Expression.Constant(value, typeof(double)))
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

using System;

namespace WallpaperGenerator.Formulas.Operators.Arithmetic
{
    public class Cbrt : UnaryOperator
    {
        private const double c1_3 = 1.0/3.0;

        public override Func<double> Evaluate(params Func<double>[] operands)
        {
            Func<double> op0 = operands[0];
            return () => Math.Pow(op0(), c1_3);
        }

        public override Func<double> Evaluate(params ZeroArityOperator[] operands)
        {
            ZeroArityOperator op0 = operands[0];
            return () => Math.Pow(op0.Value, c1_3);
        }
    }
}
